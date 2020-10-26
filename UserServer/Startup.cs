using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using UserServer.Models;

namespace UserServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TokenSettings>(
                Configuration.GetSection(nameof(TokenSettings)));
            services.AddSingleton<ITokenSettings>(sp =>
                sp.GetRequiredService<IOptions<TokenSettings>>().Value);

            services.AddControllers();

            /*services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });*/

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;  //"MyJwtScheme"; //JwtBearerDefaults.AuthenticationScheme; //
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie()
                /*.AddJwtBearer("MyJwtScheme", config =>
                {
                    var secretBytes = Encoding.UTF8.GetBytes(Configuration["TokenSettings:Secret"]);
                    var key = new SymmetricSecurityKey(secretBytes);

                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["TokenSettings:Issuer"],
                        ValidAudience = Configuration["TokenSettings:Audience"],
                        IssuerSigningKey = key
                    };
                })*/
                .AddOpenIdConnect(options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; //"MyJwtScheme"; //JwtBearerDefaults.AuthenticationScheme; //
                    options.Authority = Configuration["OpenIdConnectSettings:Authority"];
                    options.RequireHttpsMetadata = true;
                    options.ClientId = Configuration["OpenIdConnectSettings:ClientId"];
                    options.ClientSecret = Configuration["OpenIdConnectSettings:ClientSecret"];
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.SaveTokens = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "groups",
                        ValidateIssuer = true
                    };
                    var signinRedirectUrl = Configuration["OpenIdConnectSettings:SigninRedirectUrl"];
                    options.Events.OnRedirectToIdentityProvider = n =>
                    {
                        n.ProtocolMessage.RedirectUri = signinRedirectUrl;
                        return Task.CompletedTask;
                    };
                    options.Events.OnUserInformationReceived = userInfo =>
                    {
                        // Can view User information in the debugger
                        return Task.CompletedTask;
                    };
                });

            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseForwardedHeaders();

            app.Use(async (context, next) =>
            {
                // Get the original Uri
                context.Request.Headers.TryGetValue("X-Forwarded-Uri", out var originalUri);

                // Call the next delegate/middleware in the pipeline
                await next();
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
