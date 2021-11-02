using ArmsHttpClient;
using AuthenticationHttpClient;
using EmployeeApi.Domain;
using EmployeeApi.Infra;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Employee
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IAuthenticationApi authenticationApi;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthenticationServiceClient(Configuration);
            services.AddArmsServiceClient(
                Configuration, 
                new ArmsCredentials(
                    Environment.GetEnvironmentVariable("UserArmsLogin"),
                    Environment.GetEnvironmentVariable("UserArmsPassword"),
                    "atrema"));
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

            services.AddPracticeManagementContext(Configuration);
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddSingleton<IADManagment, ADManagment>();
            services.AddSingleton<IRolesManagment, RolesManagment>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new Info { Title = "You api title", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                        {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                    }
                });

            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            authenticationApi = app.ApplicationServices.GetService<IAuthenticationApi>();

            app.UseSwagger();
            app.UseSwaggerUI();

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
            });
        }

        private JwtSecurityToken SignatureValidator(string token, TokenValidationParameters parameters)
        => authenticationApi.ValidateTokenAsync(token).GetAwaiter().GetResult() switch
        {
            true => new JwtSecurityToken(token),
            false => null
        };
    }
}
