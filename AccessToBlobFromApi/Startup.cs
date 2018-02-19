using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.IdentityModel.Tokens.Jwt;
using OktaApi.AuthHandlers;

namespace OktaApi
{
    public class Startup
    {
        public IConfiguration Configuration;

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMvc();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = Configuration["okta:authority"];
                options.Audience = Configuration["okta:audience"];
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.ViewDocumentScope,
                    policy => policy.Requirements.Add(new ViewDocumentScopeRequirement(Constants.ViewDocumentScope, Configuration["okta:authority"])));
            });

            services.AddSingleton<IAuthorizationHandler, ViewDocumentScopeHandler>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("viewdocument-v1", new Info()
                {
                    Version = "1.0.0",
                    Title = "View Document"
                });
            });

            services.AddSingleton(Configuration);

            Constants.StorageConnectionString = Configuration["StorageConnectionString"];
            Constants.Scope = Configuration["okta:ScopeKey"];
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRewriter(new RewriteOptions().AddRedirect("^$", "swagger"));

            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/viewdocument-v1/swagger.json", "View Document Swagger"));
        }
    }
}
