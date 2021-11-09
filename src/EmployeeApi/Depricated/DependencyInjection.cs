using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EmployeeApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddADManagment(
         this IServiceCollection services,
         IConfiguration configuration)
        {
            var options = configuration
                      .GetSection(ADManagmentOptions.SectionName)
                      .Get<ADManagmentOptions>();

            if (options is null)
            {
                throw new InvalidOperationException(
                    $"Configuration options {ADManagmentOptions.SectionName} is absent");
            }

            services.Configure<ADManagmentOptions>(configuration.GetSection(ADManagmentOptions.SectionName));

            return services.AddScoped<IADManagment, ADManagment>();
        }
    }
}
