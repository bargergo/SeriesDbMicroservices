using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SeriesAndEpisodes.Models;
using System;

namespace SeriesAndEpisodes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            using (var scope = webHost.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<SeriesDbContext>();
                while (true)
                {
                    try
                    {
                        dbContext.TestConnection().Wait();
                        break;
                    }
                    catch (Exception e)
                    {
                        logger.LogInformation($"Waiting for db... cause:{e.Message}");
                        System.Threading.Thread.Sleep(1000);
                    }
                }

                var bus = scope.ServiceProvider.GetRequiredService<IBusControl>();
                var lifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
                var busHandle = TaskUtil.Await(() => bus.StartAsync());
                lifetime.ApplicationStopping.Register(() => busHandle.Stop());
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
