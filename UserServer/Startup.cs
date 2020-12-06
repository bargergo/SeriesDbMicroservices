using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using UserServer.Database;
using UserServer.Interfaces;
using UserServer.Models;
using UserServer.Services;

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

            services.AddDbContext<UserDbContext>(o =>
                o.UseSqlServer(Configuration.GetConnectionString("DbConnection"), options => options.EnableRetryOnFailure()));

            services.AddTransient<IUserService, UserService>();

            services.AddControllers();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;  //"MyJwtScheme"; //JwtBearerDefaults.AuthenticationScheme; //
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie()
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
                })
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
                    options.Events.OnRedirectToIdentityProvider = context =>
                    {
                        context.ProtocolMessage.RedirectUri = signinRedirectUrl;
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
