using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EmployeeApi.Infra
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPracticeManagementContext(
          this IServiceCollection services,
          IConfiguration configuration)
        => services
            .AddDbContext<PracticeManagementContext>(options =>
                {
                    var connectionString = configuration.GetConnectionString("PracticeManagement")
                         ?? throw new ArgumentNullException("ConnectionString PracticeManagement is absent");

                    var userDb = Environment.GetEnvironmentVariable("UserDB") 
                        ?? throw new ArgumentNullException("Environment variable UserDB is absent");

                    var userDbPassword = Environment.GetEnvironmentVariable("UserDBPassword") 
                        ?? throw new ArgumentNullException("Environment variable UserDBPassword is absent");

                    options.UseSqlServer($"{connectionString};User Id={userDb};Password={userDbPassword}",
                    sqlServerOptions =>
                    {
                        sqlServerOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    });
                }
            );
  
    }
}
