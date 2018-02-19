using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;

namespace OktaWebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options =>
                {
                    options.ClientId = Configuration["okta:ClientId"];
                    options.ClientSecret = Configuration["okta:ClientSecret"];
                    options.Authority = Configuration["okta:Authority"];
                    options.CallbackPath = "/authorization-code/callback";
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    options.UseTokenLifetime = false;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add(Configuration["okta:CustomScope"]);
                });

            services.AddSingleton(Configuration);

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{Id?}");
            });

            app.Run(async ctx => await ctx.Response.WriteAsync("Welcome to File download!"));
        }
    }
}
