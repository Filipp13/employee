using ADManager;
using ArmsHttpClient;
using AuthenticationHttpClient;
using Employee.Api.Domain;
using Employee.Api.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.DirectoryServices.Protocols;

namespace Employee.Api
{
    public static class DependencyInjection
    {
        private const string databaseTag = "database";

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
