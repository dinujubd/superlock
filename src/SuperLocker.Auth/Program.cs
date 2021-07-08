using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SuperLocker.Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    // webBuilder.UseUrls("http://localhost:5005, https://localhost:5006");
                });
    }
}
