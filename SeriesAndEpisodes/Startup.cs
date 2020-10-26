using AutoMapper;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SeriesAndEpisodes.MessageQueue;
using SeriesAndEpisodes.Models;
using SeriesAndEpisodes.Services;
using System;
using System.Text;

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
                var config = Configuration.GetSection(nameof(MessageQueueSettings)).Get<MessageQueueSettings>();
                x.AddConsumer<SeriesRatingChangedEventHandler>();
                x.AddBus(context =>
                    Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {

                        var host = cfg.Host(new Uri($"rabbitmq://{config.Hostname}:/"),
                            hostConfig =>
                            {
                                hostConfig.Username(config.Username);
                                hostConfig.Password(config.Password);
                            });
                        cfg.ReceiveEndpoint("SeriesRatingUpdateQueue", ep =>
                        {
                            ep.ConfigureConsumer<SeriesRatingChangedEventHandler>(context);
                        });
                    })
                );
                EndpointConvention.Map<IDummyMessage>(new Uri($"rabbitmq://{config.Hostname}:/dummy"));
            });

            services.AddAuthentication("MyJwtScheme")
                .AddJwtBearer("MyJwtScheme", config =>
                {
                    var secretBytes = Encoding.UTF8.GetBytes(Configuration["TokenSettings:Secret"]);
                    var key = new SymmetricSecurityKey(secretBytes);

                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["TokenSettings:Issuer"],
                        ValidAudience = Configuration["TokenSettings:Audience"],
                        IssuerSigningKey = key
                    };
                });

            services.Configure<SeriesDbSettings>(
                Configuration.GetSection(nameof(SeriesDbSettings)));
            services.AddSingleton<ISeriesDbSettings>(sp =>
                sp.GetRequiredService<IOptions<SeriesDbSettings>>().Value);

            services.AddSingleton<SeriesDbContext>();

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
            app.UseAuthentication();
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
