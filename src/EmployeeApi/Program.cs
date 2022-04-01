using AspNetCore.Logger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace Employee.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        => Host
            .CreateDefaultBuilder(args)
            .UseLogger()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(5000, listenOptions =>
                           listenOptions.Protocols = HttpProtocols.Http1);

                    options.ListenAnyIP(5002, listenOptions =>
                            listenOptions.Protocols = HttpProtocols.Http2);
                });
                webBuilder.UseStartup<Startup>();
            });

    }
}
