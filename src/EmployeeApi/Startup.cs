using AuthenticationHttpClient;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EmployeeApi.Tests")]
namespace EmployeeApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IAuthenticationApi? authenticationApi;

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

            services.AddSwagger();

            services.AddHealthCheck();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            authenticationApi = app.ApplicationServices.GetService<IAuthenticationApi>()
                ?? throw new Exception("AuthenticationApi is not resolved");

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/employee/swagger/{documentname}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api/employee/swagger/v1/swagger.json", "Employee API");
                c.RoutePrefix = "api/employee/swagger";
            });

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
