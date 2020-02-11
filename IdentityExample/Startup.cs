using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityExample.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityExample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(cfg=>cfg.UseInMemoryDatabase("Memory"));

            services.AddIdentity<IdentityUser, IdentityRole>(cfg=> {
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredLength = 4;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
            
            })

                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(cfg=>
            {
                cfg.Cookie.Name = "id.cookie";
                cfg.LoginPath = "/home/login";
            });
    

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
