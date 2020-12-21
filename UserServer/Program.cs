using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using UserServer.Database;

namespace UserServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            using (var scope = webHost.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                while (true)
                {
                    try
                    {
                        scope.ServiceProvider.GetRequiredService<UserDbContext>().Database.Migrate();
                        break;
                    }
                    catch (Exception e)
                    {
                        logger.LogInformation($"Waiting for db... cause:{e.Message}");
                        System.Threading.Thread.Sleep(1000);
                    }
                }

            }
            webHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
