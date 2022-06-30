using AspNetCore.Logger;
using Microsoft.AspNetCore.Hosting;
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
                webBuilder.UseStartup<Startup>();
            });

    }
}
