using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;

namespace AuthenticationHttpClient
{

    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddAuthenticationServiceClient(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            var options = configuration
                      .GetSection(AuthenticationServiceOptions.SectionName)
                      .Get<AuthenticationServiceOptions>();

            if (options is null)
            {
                throw new InvalidOperationException(
                    $"Configuration options {AuthenticationServiceOptions.SectionName} is absent");
            }

            services.Configure<AuthenticationServiceOptions>(configuration.GetSection(AuthenticationServiceOptions.SectionName));

            return services
              .AddHttpClient<IAuthenticationApi, AuthenticationApi>()
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
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()
              .WaitAndRetryAsync(retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryDelay, retryAttempt)));
        }
    }
}