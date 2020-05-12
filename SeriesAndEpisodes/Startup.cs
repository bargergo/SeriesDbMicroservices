using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SeriesAndEpisodes.MessageQueue;
using SeriesAndEpisodes.Models;
using SeriesAndEpisodes.Services;

namespace SeriesAndEpisodes
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<SeriesRatingChangedEventHandler>();
                x.AddBus(provider =>
                    Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var host = cfg.Host(new Uri($"rabbitmq://message-queue:/"),
                            hostConfig =>
                            {
                                hostConfig.Username("guest");
                                hostConfig.Password("guest");
                            });
                        cfg.ReceiveEndpoint("SeriesRatingUpdateQueue", ep =>
                        {
                            ep.Consumer<SeriesRatingChangedEventHandler>();
                        });
                    }));
                EndpointConvention.Map<IDummyMessage>(new Uri("rabbitmq://rabbitmq:/dummy"));
            });
            services.Configure<RatingsServiceSettings>(
                Configuration.GetSection(nameof(RatingsServiceSettings)));
            services.AddSingleton<IRatingsServiceSettings>(sp =>
                sp.GetRequiredService<IOptions<RatingsServiceSettings>>().Value);
            services.AddHttpClient<RatingsService>();

            services.Configure<SeriesDbSettings>(
                Configuration.GetSection(nameof(SeriesDbSettings)));
            services.AddSingleton<ISeriesDbSettings>(sp =>
                sp.GetRequiredService<IOptions<SeriesDbSettings>>().Value);

            services.AddSingleton<SeriesService>();
            services.AddSingleton<FileService>();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SeriesAndEpisodes API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SeriesAndEpisodes API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var bus = app.ApplicationServices.GetService<IBusControl>();
            var busHandle = TaskUtil.Await(() => bus.StartAsync());
            lifetime.ApplicationStopping.Register(() => busHandle.Stop());
        }
    }
}
