using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using wishlist;
using wishlist.Models.Identity;
using wishlist.Services;
using wishlist.Services.BlobService;
using wishlist.Services.EventService;
using wishlist.Services.GiftService;
using wishlist.Services.Profiles;
using wishlist.Services.User;

namespace wishlist
{
    public class Startup
    {
        private IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                services.AddDbContext<ApplicationDbContext>(build =>
                {
                    build.UseMySql(configuration.GetConnectionString("AzureConnection"));
                });
                // Automatically perform database migration
                services.BuildServiceProvider().GetService<ApplicationDbContext>().Database.Migrate();
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(build =>
                {
                    build.UseMySql(configuration.GetConnectionString("DefaultConnection"));
                });
            }
            services.SetUpAutoMapper();
            services.AddMvc();
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "111294217428-tjdmd0tb6uo4rq0fdn64jkskbp89f7na.apps.googleusercontent.com";
                    options.ClientSecret = "yJ0vP6rfV1j9V_WWW61pduHQ";
                });

            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEventService, EventService>();
<<<<<<< HEAD
            services.AddTransient<IGiftService, GiftService>();
=======
            services.AddTransient<IUserService, UserService>();
>>>>>>> 5c83eed36c4572415171c1791760deebab226b7b
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
