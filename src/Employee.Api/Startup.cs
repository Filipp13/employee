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
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            currentEnvironment = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IAuthenticationApi? authenticationApi { get; set; }

        private IWebHostEnvironment currentEnvironment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            Configuration["SwaggerGenerator:SwaggerDocs:v1:Title"] = "Employee.Api";
            Configuration["SwaggerGenerator:SwaggerDocs:v1:Description"] = "Сервис работников";

            Configuration["SwaggerGenerator:SwaggerDocs:v1:TermsOfService"] = "https://cis-confl.deloitteresources.com/display/CPT/PMP";

            Configuration["SwaggerGenerator:SwaggerDocs:v1:Contact:Name"] = "Employee.Api";

            Configuration["SwaggerGenerator:SwaggerDocs:v1:Contact:Email"] = "fantipov@deloitte.ru";

            Configuration["SwaggerGenerator:SwaggerDocs:v1:Contact:Url"] = "https://cis-confl.deloitteresources.com/display/CPT/PMP";

            Configuration["SwaggerUI:RoutePrefix"] = "api/employee/swagger";

            Configuration["SwaggerUI:ConfigObject:Urls:0:Url"] = "/api/employee/swagger/v1/swagger.json";

            Configuration["SwaggerUI:ConfigObject:Urls:0:Name"] = "Employee.Api";

            Configuration["Swagger:RouteTemplate"] = "api/employee/swagger/{documentname}/swagger.json";



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
