using AspNetCore.Metrics;
using AspNetCore.SwaggerGen;
using AuthenticationHttpClient;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SP.Core.AspNetCore.SwaggerUI;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EmployeeApiTests")]
namespace Employee.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            currentEnvironment = env;

            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        private IAuthenticationApi? authenticationApi { get; set; }

        private IWebHostEnvironment currentEnvironment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            services.Configure<CacheSettings>(Configuration.GetSection("CacheSettings"));

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "Auth Service",
                        NameClaimType = "Login",
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = false,
                        ValidateLifetime = false,
                        SignatureValidator = SignatureValidator
                    };
                });


            services.AddInternalServices(Configuration);

            services.AddControllers();

            services.AddSwagger(currentEnvironment, Configuration);
            services.ConfigureSwaggerUI(currentEnvironment, Configuration);

            services.AddHealthCheck();
            services.AddPrometheusMetrics(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            authenticationApi = app.ApplicationServices.GetService<IAuthenticationApi>()
                ?? throw new Exception("AuthenticationApi is not resolved");

            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.UseHealthCheck();
            });
        }

        private JwtSecurityToken SignatureValidator(string token, TokenValidationParameters parameters)
        => authenticationApi!.ValidateTokenAsync(token).GetAwaiter().GetResult() switch
        {
            true => new JwtSecurityToken(token),
            false => null!
        };
    }
}
