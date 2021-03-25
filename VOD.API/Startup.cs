using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VOD.API.Services;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Database.Context;
using VOD.Database.Service;

namespace VOD.API
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

            services.AddControllers();
            services.AddScoped<IDbReadService, DbReadService>();
            services.AddScoped<ICrudService, CrudService>();

            var config = new MapperConfiguration(configure =>
            {
                configure.CreateMap<Video, VideoDTO>().ForMember(destinationMember
                    => destinationMember.Url, source =>
                    source.MapFrom(s => s.Url));

                configure.CreateMap<VideoDTO, Video>().ForMember(destinationMember
                    => destinationMember.Url, source =>
                    source.MapFrom(s => s.Url));

                configure.CreateMap<Instructor, InstructorDTO>().ForMember(destinationMember =>
                    destinationMember.InstructorName, source =>
                    source.MapFrom(s => s.Name)).ForMember(destinationMember =>
                    destinationMember.InstructorDescription, source =>
                    source.MapFrom(s => s.Description)).ForMember(destinationMember =>
                    destinationMember.InstructorAvatar, source =>
                    source.MapFrom(s => s.Thumbnail));

                configure.CreateMap<InstructorDTO, Instructor>().ForMember(destinationMember =>
                    destinationMember.Name, source =>
                    source.MapFrom(s => s.InstructorName)).ForMember(destinationMember =>
                    destinationMember.Description, source =>
                    source.MapFrom(s => s.InstructorDescription)).ForMember(destinationMember =>
                    destinationMember.Thumbnail, source =>
                    source.MapFrom(s => s.InstructorAvatar));


                configure.CreateMap<Download, DownloadDTO>().ForMember(destinationMember =>
                    destinationMember.DownloadTitle, source =>
                    source.MapFrom(s => s.Title)).ForMember(destinationMember =>
                    destinationMember.DownloadUrl, source =>
                    source.MapFrom(s => s.Url));

                configure.CreateMap<DownloadDTO, Download>().ForMember(destinationMember =>
                    destinationMember.Title, source =>
                    source.MapFrom(s => s.DownloadTitle)).ForMember(destinationMember =>
                    destinationMember.Url, source =>
                    source.MapFrom(s => s.DownloadUrl));

                configure.CreateMap<Course, CourseDTO>().ForMember(destinationMember =>
                    destinationMember.CourseDescription, source =>
                    source.MapFrom(s => s.Description)).ForMember(destinationMember =>
                    destinationMember.CourseTitle, source =>
                    source.MapFrom(s => s.Title)).ForMember(destinationMember =>
                    destinationMember.CourseImageUrl, source =>
                    source.MapFrom(s => s.ImageUrl)).ForMember(destinationMember =>
                    destinationMember.Id, source =>
                    source.MapFrom(s => s.Id));


                configure.CreateMap<CourseDTO, Course>().ForMember(destinationMember =>
                    destinationMember.Description, source =>
                    source.MapFrom(s => s.CourseDescription)).ForMember(destinationMember =>
                    destinationMember.Title, source =>
                    source.MapFrom(s => s.CourseTitle)).ForMember(destinationMember =>
                    destinationMember.ImageUrl, source =>
                    source.MapFrom(s => s.CourseImageUrl)).ForMember(destinationMember =>
                    destinationMember.Id, source =>
                    source.MapFrom(s => s.Id));

                configure.CreateMap<Module, ModuleDTO>().ForMember(destinationMember =>
                    destinationMember.ModuleTitle, source =>
                    source.MapFrom(s => s.Title));

                configure.CreateMap<ModuleDTO, Module>().ForMember(destinationMember =>
                    destinationMember.Title, source =>
                    source.MapFrom(s => s.ModuleTitle));
            });
            var mappia = config.CreateMapper();
            services.AddSingleton(mappia);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}