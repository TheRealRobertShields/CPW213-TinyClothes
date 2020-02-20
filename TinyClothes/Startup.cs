using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TinyClothes.Data;
using Microsoft.EntityFrameworkCore;

namespace TinyClothes
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void ConfigDbContext(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(""); // <<< CONNECTION STRING GOES IN THE QUOTES!!!
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // services.AddDbContext<StoreContext>(ConfigDbContext);

            string connection = Configuration.GetConnectionString("ClothesDB");

            // Same as above using lambda notation
            services.AddDbContext<StoreContext>(options => options.UseSqlServer(connection)); // <<< CONNECTION STRING GOES IN THE QUOTES!!!

            // Add and configure session
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.Name = ".TinyClothes.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.IsEssential = true; // Session cookie always created even if user does not accept cookie policy
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseAuthorization();

            // Allows session data to be accessed.
            app.UseSession(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
