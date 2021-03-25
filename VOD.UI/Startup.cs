using System;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Common.Services;
using VOD.Database.Context;
using VOD.Database.Service;
using VOD.UI.Services;

namespace VOD.UI
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
            services.AddDbContext<VodContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<VodUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<VodContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<IDbReadService, DbReadService>();
            services.AddScoped<IUiReadService, UiReadService>();

            var config = new MapperConfiguration(configure =>
            {
                configure.CreateMap<Video, VideoDTO>().ForMember(destinationMember
                    => destinationMember.Url, source =>
                    source.MapFrom(s => s.Url));

                configure.CreateMap<Instructor, InstructorDTO>().ForMember(destinationMember =>
                    destinationMember.InstructorName, source =>
                    source.MapFrom(s => s.Name)).ForMember(destinationMember =>
                    destinationMember.InstructorDescription, source =>
                    source.MapFrom(s => s.Description)).ForMember(destinationMember =>
                    destinationMember.InstructorAvatar, source =>
                    source.MapFrom(s => s.Thumbnail));

                configure.CreateMap<Download, DownloadDTO>().ForMember(destinationMember =>
                    destinationMember.DownloadTitle, source =>
                    source.MapFrom(s => s.Title)).ForMember(destinationMember =>
                    destinationMember.DownloadUrl, source =>
                    source.MapFrom(s => s.Url));

                configure.CreateMap<Course, CourseDTO>().ForMember(destinationMember =>
                    destinationMember.CourseDescription, source =>
                    source.MapFrom(s => s.Description)).ForMember(destinationMember =>
                    destinationMember.CourseTitle, source =>
                    source.MapFrom(s => s.Title)).ForMember(destinationMember =>
                    destinationMember.CourseImageUrl, source =>
                    source.MapFrom(s => s.ImageUrl)).ForMember(destinationMember =>
                    destinationMember.Id, source =>
                    source.MapFrom(s => s.Id));


                configure.CreateMap<Module, ModuleDTO>().ForMember(destinationMember =>
                    destinationMember.ModuleTitle, source =>
                    source.MapFrom(s => s.Title)).ReverseMap();
            });
            var mappia = config.CreateMapper();
            services.AddSingleton(mappia);

            //configuration for api
            services.AddHttpClient("AdminClient", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5500");
                client.Timeout = new TimeSpan(0, 0, 45);
                client.DefaultRequestHeaders.Clear();
            }).ConfigurePrimaryHttpMessageHandler(handler => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });
            services.AddScoped<IAPIService, APIService>();
            services.AddScoped<IHttpClientFactoryService, HttpClientFactoryService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, VodContext db)
        {
            db.SeedMembershipData();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}