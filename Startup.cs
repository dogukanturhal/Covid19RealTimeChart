using DogukanTURHAL.Covid19.API.Context;
using DogukanTURHAL.Covid19.API.Hubs;
using DogukanTURHAL.Covid19.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DogukanTURHAL.Covid19.API
{

    /*
     * Bu proje katmanlý mimariye geçebilirdi.
     * Kafamda projeyi düþündükten sonra doðaçlama bir þekilde geliþtirdim.
     * Katmanlý mimari yapmýþ olsaydým.
     * Core Layer - Entityler için katman
     * Data Access Layer - Ormler ve db baðlantýsý katmaný
     * Business Layer - Ýþ kodlarýnýn oluþturulduðu katman
     * API - controllerin ve api için configurasyonlarýn tanýmladýðý katman
     * 
     * Microservis bir yapý olsaydý domain driven design kullanýrdým.
     * Domain Layer- Entityler için katman
     * Application Layer- iþ kodlarý için katman
     * Infrastructure - orm ve db baðlantý katmaný
     * API veya presentation layer - controllerin ve api için configurasyonlarýn tanýmladýðý katman
     */
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /*
            Bu bölümde servis olarak DbContext'i tanýmladým. Db Context connection'ý appsettings.json içerisinde bulunan ConnectionString'ten almaktadýr.
            Cors ekleyerek baðlantý yaptýðým url'lerin giriþine izin verdim.
         */
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration["ConnectionString"]);
            });

            /*
             * Bu bölümde dependency injection kullandým.
             * AddScoped kullanmamým sebebi her bir web requestinden bir instance oluþturmasý için eðer farklý bir web request gerçekleþirse yeni bir instance oluþturabilmesi.
             * AddTransient kullanmýþ olsaydým her servis istendiðinde yeni instance üretecekti.
             * AddSingleton kullanmýþ olsaydým proje ilk baþladýðýnda bir instace üretecekti ve her requestte o instance'ý kullancaktý
             */
            services.AddScoped<CovidService>();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("https://localhost:44374", "http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });
            services.AddSignalR();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DogukanTURHAL.Covid19.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DogukanTURHAL.Covid19.API v1"));
            }

            app.UseHttpsRedirection();
            /*
             Tanýmladýðým CorsPolicy'yý uygulama içerisine configure ettim.
            Kullandýðým websocketi CovidHub route'u üzerinden eriþimini saðladým.
             */
            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<CovidHub>("/CovidHub");
            });
        }
    }
}
