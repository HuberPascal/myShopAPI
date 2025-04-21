using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using myShopAPI.Data;
using Microsoft.OpenApi.Models;
using AutoMapper;
using myShopAPI.Models;
using myShopAPI.Transport;
using myShopAPI.Mappings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace myShopAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // CORS-Regeln hinzufügen
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost", policy =>
                    policy.WithOrigins("http://localhost:4200")  // Erlaube Anfragen von localhost:4200
                          .AllowAnyMethod()    // Erlaube alle HTTP-Methoden (GET, POST, etc.)
                          .AllowAnyHeader());  // Erlaube alle Header (einschließlich Content-Type)
            });

            // Füge die DbContext- und Connection-String-Konfiguration hinzu
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            
            // builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                // .AddEntityFrameworkStores<ApplicationDbContext>();

            
            // AutoMapper manuell konfigurieren
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                // Hier alle Mapping-Profile hinzufügen
                cfg.AddProfile<CartMappingProfile>();
            });
            
            builder.Services.AddIdentityCore<IdentityUser<Guid>>()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddApiEndpoints();
            
            builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);

            builder.Services.AddAuthorization();
            builder.Services.AddAuthorizationBuilder();

            // Als Singleton im DI-Container registrieren
            builder.Services.AddSingleton<IMapper>(sp => mapperConfig.CreateMapper());

            // Füge die Controller und Swagger-Dienste hinzu
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Mein API", Version = "v1" });
            });

            var app = builder.Build();

            // Aktiviere Swagger und SwaggerUI im Entwicklungsmodus
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meine API v1");
                }); 
            }

            // WICHTIG: CORS aktivieren
            app.UseCors("AllowLocalhost");  // Hier CORS aktivieren

            app.UseHttpsRedirection();
            
            // Developer Exception Page nur im Entwicklungsmodus
            if (app.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}