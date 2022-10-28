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
using Microsoft.EntityFrameworkCore;
using MusicStore.Models;

namespace MusicStore
{
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
            services.AddRouting(options => {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
            });

            services.AddControllersWithViews();

            services.AddDbContext<MusicContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MusicContext")));
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

            app.UseEndpoints(endpoints =>
            {//The more specific/customized route gets placed above the default endpoint.

                //you would setup your area here
                /*endpoints.MapControllerRoute(
                    name: "admin",
                    areaName: "Admin", //important because this is where you associate your controllers with the area
                    pattern: "Admin/{controller=Home)/{action=Index}/{id?};)*/
                // YOU HAVE TO HAVE AREAS/ADMIN IN YOUR AREA ADMIN VIEWS IN ORDER FOR THE ROUTING TO WORK.

                //sort-album has 2 parameters
                endpoints.MapControllerRoute( 
                    name: "sort_album",
                    //can sort by either artist or genre
                    //sort=0, not sorted; sort=1, sorted ascending; sort=2, sorted descending
                    pattern: "{controller=Album}/{action=Index}/{ArtistSort?}/{GenreSort?}");

                endpoints.MapControllerRoute(
                    name: "sort_artist",
                    //id=0, not sorted; id=1, sorted ascending; id=2, sorted descending
                    pattern: "{controller=Artist}/{action=Index}/{ArtistSort?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
