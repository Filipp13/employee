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
                    options.UseSqlServer(configuration.GetConnectionString("PracticeManagement"),
                    sqlServerOptions =>
                    {
                        sqlServerOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    });
                }
            );
  
    }
}
