using System;
using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.FileAccess;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public class FakeStartup
    {
        public FakeStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            var dbGuid = Guid.NewGuid().ToString();
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite("Data Source=" + dbGuid + ".db");
            });

            //services.AddAutoMapper(GetType().Assembly);
            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews()
                    .AddApplicationPart(typeof(Startup).Assembly);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IArtistRepository, ArtistRepository>();
            services.AddScoped<ISongRepository, SongRepository>();
            services.AddScoped<IStorageRepository, StorageRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<ISongScheduleRepository, SongScheduleRepository>();

            services.AddScoped<IArtistService, ArtistService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ISongService, SongService>();
            services.AddScoped<IStorageService, StorageService>();

            services.AddScoped<IFileClient, FileClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/{pathStr?}");
            });

            var contentRoot = env.ContentRootPath;

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            using(var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
            {
                var scheduleService = serviceScope.ServiceProvider.GetService<IScheduleService>();
                
                if (dbContext == null)
                {
                    throw new NullReferenceException("Cannot get instance of dbContext");
                }

                if (dbContext.Database.GetDbConnection().ConnectionString.ToLower().Contains("my.db"))
                {
                    throw new Exception("LIVE SETTINGS IN TESTS!");
                }

                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                DemoData.Initialize(dbContext, scheduleService);
            }
        }
    }
}