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
     * Bu proje katmanl� mimariye ge�ebilirdi.
     * Kafamda projeyi d���nd�kten sonra do�a�lama bir �ekilde geli�tirdim.
     * Katmanl� mimari yapm�� olsayd�m.
     * Core Layer - Entityler i�in katman
     * Data Access Layer - Ormler ve db ba�lant�s� katman�
     * Business Layer - �� kodlar�n�n olu�turuldu�u katman
     * API - controllerin ve api i�in configurasyonlar�n tan�mlad��� katman
     * 
     * Microservis bir yap� olsayd� domain driven design kullan�rd�m.
     * Domain Layer- Entityler i�in katman
     * Application Layer- i� kodlar� i�in katman
     * Infrastructure - orm ve db ba�lant� katman�
     * API veya presentation layer - controllerin ve api i�in configurasyonlar�n tan�mlad��� katman
     */
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /*
            Bu b�l�mde servis olarak DbContext'i tan�mlad�m. Db Context connection'� appsettings.json i�erisinde bulunan ConnectionString'ten almaktad�r.
            Cors ekleyerek ba�lant� yapt���m url'lerin giri�ine izin verdim.
         */
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration["ConnectionString"]);
            });

            /*
             * Bu b�l�mde dependency injection kulland�m.
             * AddScoped kullanmam�m sebebi her bir web requestinden bir instance olu�turmas� i�in e�er farkl� bir web request ger�ekle�irse yeni bir instance olu�turabilmesi.
             * AddTransient kullanm�� olsayd�m her servis istendi�inde yeni instance �retecekti.
             * AddSingleton kullanm�� olsayd�m proje ilk ba�lad���nda bir instace �retecekti ve her requestte o instance'� kullancakt�
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
             Tan�mlad���m CorsPolicy'y� uygulama i�erisine configure ettim.
            Kulland���m websocketi CovidHub route'u �zerinden eri�imini sa�lad�m.
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
