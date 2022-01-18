using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace Employee.Api
{
    public static class ConfigExtension
    {
        public static IConfigurationBuilder AddConfigFile(
            this IConfigurationBuilder configurationBuilder, 
            IWebHostEnvironment env)
        => Environment.GetEnvironmentVariable("Dev") is null ? 
            configurationBuilder : configurationBuilder.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
    }
}
