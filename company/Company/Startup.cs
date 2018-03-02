using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Company
{
    using Core;
    using Data;
    using Microsoft.AspNetCore.Authentication.JwtBearer;

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
            services.AddMvcCore().AddJsonFormatters();
            services.AddOptions();
            services.Configure<Conf>(Configuration);

            var mongoConnection = this.Configuration.GetConnectionString("Mongo");
            services.AddSingleton<MongoDataProvider>(new MongoDataProvider(mongoConnection));

            services.AddSingleton<CompanyService>();
            services.AddSingleton<CompanyUserService>();


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    //TODO: Get it from another source.(it should be changed autiomatically)
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("B69B6D11-215C-467D-B51D-90CDFEA67336")),
                    ValidateLifetime = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Svc",
                    policy => policy.Requirements.Add(new SvcRquirement()));
                options.AddPolicy("SvcUser",
                    policy => policy.Requirements.Add(new UserRequirement()));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
