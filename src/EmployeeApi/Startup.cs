using AspNetCore.Cache;
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

[assembly: InternalsVisibleTo("Employee.Api.Tests")]
namespace Employee.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            currentEnvironment = env;

            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddConfigFile(env);

            configuration = builder.Build();
        }

        public IConfiguration configuration { get; }

        private IAuthenticationApi? authenticationApi { get; set; }

        private IWebHostEnvironment currentEnvironment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddAspNetMediatrCache(configuration.GetSection("CacheSettings").Get<CacheSettings>());

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


            services.AddInternalServices(configuration);

            services.AddGrpc();
            services.AddControllers();

            services.AddSwagger(currentEnvironment, configuration);
            services.ConfigureSwaggerUI(currentEnvironment, configuration);

            services.AddHealthCheck();
            services.AddPrometheusMetrics(configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            authenticationApi = app.ApplicationServices.GetService<IAuthenticationApi>()
                ?? throw new ArgumentNullException(nameof(authenticationApi), "AuthenticationApi is not resolved");

            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<EmployeeGrpcService>();
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
