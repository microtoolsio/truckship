using System;
using System.Net;
using System.Threading.Tasks;
using Gateway.Configs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Gateway
{
    public class Startup
    {
        private IHostingEnvironment CurrentEnvironment { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("routes.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            CurrentEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<GatewaySessionStore>();
            services.AddSingleton<SvcRouteTable>();
            services.AddSingleton<SecretStorage>();
            services.AddSingleton<ClientHelper>();

            // Add framework services.
            services.AddMvc().AddJsonOptions(
                options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            var redisConnection = this.Configuration.GetConnectionString("Redis");

            //NOTE: Get from appsettings
            services.AddDistributedRedisCache(option =>
            {
                option.Configuration = redisConnection;
                option.InstanceName = "master";
            });

            services.Configure<RouteConfig>(Configuration);
            services.Configure<AppSettings>(Configuration.GetSection("Settings"));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.SecurePolicy = CurrentEnvironment.EnvironmentName == EnvironmentName.Development ?
                    CookieSecurePolicy.None : CookieSecurePolicy.Always;
                    options.Events.OnRedirectToLogin = (context) =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return Task.CompletedTask;
                    };
                    var scopeFactory = services
                        .BuildServiceProvider()
                        .GetRequiredService<IServiceScopeFactory>();

                    using (var scope = scopeFactory.CreateScope())
                    {
                        var provider = scope.ServiceProvider;
                        options.SessionStore = provider.GetRequiredService<GatewaySessionStore>();
                    }
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();

            await app.ApplicationServices.GetService<SecretStorage>().Init();
        }
    }
}
