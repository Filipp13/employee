using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace Employee.Api.ServiceClient
{

    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddEmployeeApiServiceClient(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            var options = configuration
                      .GetSection(EmployeeServiceOptions.SectionName)
                      .Get<EmployeeServiceOptions>();

            if (options is null)
            {
                throw new InvalidOperationException(
                    $"Configuration options {EmployeeServiceOptions.SectionName} is absent");
            }

            services.Configure<EmployeeServiceOptions>(configuration.GetSection(EmployeeServiceOptions.SectionName));

            return services
              .AddHttpClient<IEmployeeApi, EmployeeApi>()
              .AddHeaderPropagation()
              .SetHandlerLifetime(TimeSpan.FromMinutes(1))
              .AddPolicyHandler(GetRetryPolicy(options.RetryCount, options.RetryDelay))
              .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(options.TryTimeout))
              .ConfigureHttpClient(client =>
              {
                  client.BaseAddress = new Uri(options.BaseAddress!);
                  client.Timeout = TimeSpan.FromSeconds(options.OverallTimeout);
              });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount, int retryDelay)
        => HttpPolicyExtensions
              .HandleTransientHttpError()
              .WaitAndRetryAsync(
                retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryDelay, retryAttempt)));
    }
}