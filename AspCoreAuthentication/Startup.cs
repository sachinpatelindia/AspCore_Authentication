using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspCoreAuthentication.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspCoreAuthentication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("cookie").AddCookie("cookie",m=>
            {
                m.Cookie.Name = "demo.cookie";
                m.LoginPath = "/home/authenticate";

            });
            services.AddAuthorization(config=>
            {
                // var defaultAuthBuilder=new AuthorizationPolicyBuilder():
                //var defaultAuthPolicy = defaultAuthBuilder
                //.RequireAuthenticatedUser()
                //.RequireClaim(ClaimTypes.DateOfBirth)
                //.Build();
                //config.AddPolicy("claim.dob", policyBuilder=>
                //{
                //    policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                //});
                //Gloabal policy, organazation level
                config.AddPolicy("Admin",policy=>policy.RequireClaim(ClaimTypes.Role,"Admin"));

                config.AddPolicy("claim.dob", policyBuilder =>
                {
                    policyBuilder.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
                });
            });
            services.AddScoped<IAuthorizationHandler, CustomRequireHandler>();
            services.AddControllersWithViews();
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
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
