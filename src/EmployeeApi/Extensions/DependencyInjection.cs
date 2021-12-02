using ADManager;
using ArmsHttpClient;
using AuthenticationHttpClient;
using EmployeeApi.Domain;
using EmployeeApi.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;

namespace EmployeeApi
{
    public static class DependencyInjection
    {
        private const string databaseTag = "database";

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        => services.AddSwaggerGen(c =>
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

        public static IServiceCollection AddHealthCheck(this IServiceCollection services)
        => services
            .AddHealthChecks()
            .AddDbContextCheck<PracticeManagementContext>("PracticeManagementDatabase", HealthStatus.Unhealthy, tags: new[] { databaseTag })
            //.AddDbContextCheck<PeopleContext>("PeopleContextDatabase", HealthStatus.Unhealthy, tags: new[] { databaseTag })
            .Services;


        public static IEndpointRouteBuilder UseHealthCheck(this IEndpointRouteBuilder builder)
        {
            builder.MapHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = (check) => check.Tags.Contains("only service")
            });

            builder.MapHealthChecks("/health/db", new HealthCheckOptions()
            {
                Predicate = (check) => check.Tags.Contains(databaseTag)
            });

            return builder;
        }

        public static IServiceCollection AddInternalServices(this IServiceCollection services, IConfiguration Configuration)
        => services
            .AddTransient(typeof(Entity<EmployeeAD, SearchResultEntry>), typeof(EmployeeMapper))
            .AddTransient(typeof(IADManagmentEntity<,>), typeof(ADManagment<,>))
            .AddAuthenticationServiceClient(Configuration).Services
            .AddArmsServiceClient(
                Configuration,
                new ArmsCredentials(
                    Environment.GetEnvironmentVariable("UserArmsLogin")
                        ?? throw new ArgumentNullException("UserArmsLogin"),
                    Environment.GetEnvironmentVariable("UserArmsPassword")
                        ?? throw new ArgumentNullException("UserArmsPassword"),
                    "atrema")).Services
            .AddDatabaseContext(Configuration)
            .AddTransient<IEmployeeRepository, EmployeeRepository>()
            .AddSingleton<IRolesManagment, RolesManagment>()
            .AddADManagment(Configuration, new ADManagerSecurityOptions
            {
                Login = Environment.GetEnvironmentVariable("UserADLogin")
                        ?? throw new ArgumentNullException("UserADLogin"),
                Password = Environment.GetEnvironmentVariable("UserADPassword")
                        ?? throw new ArgumentNullException("UserADPassword")
            });

    }
}
