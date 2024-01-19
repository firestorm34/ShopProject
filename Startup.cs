using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using ShopProject.Models;
using System.ComponentModel.DataAnnotations;
using ShopProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Serilog;
using Seq.Extensions.Logging;
namespace ShopProject
{
    public partial class  Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            string con = Configuration.GetConnectionString("DefaultConnection");

            //services.AddLogging(
            //});

            services.AddDbContext<ShopContext>(options => options.UseMySql(con, new MySqlServerVersion(new Version(8, 0, 11))));
            #region Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<UnitOfWork>();
            services.AddIdentity<User, Role>(options => {
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ShopContext>();
            
            //services.AddTransient(typeof(UserManager<User>), typeof(MyUserManager));
            //services.AddScoped<CurrentUser>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //var authOptions = new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //    LogoutPath = new PathString("/Account/Logout"),
            //    ExpireTimeSpan = TimeSpan.FromDays(7),
            //};
            //app.UseCookieAuthentication(authOptions);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

           
            app.UseHttpsRedirection();
            app.UseStaticFiles();
          
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
           
            app.UseEndpoints(endpoints =>
            {
//                endpoints.MapAreaControllerRoute(
//                name: "admin",
//                areaName: "admin",
//                pattern: "admin/{controller=Category}/{action=Index}"
//);
                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "{area:exists}/{controller=Category}/{action=Index}/{id?}");
            
            endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
