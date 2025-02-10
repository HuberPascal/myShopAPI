
using Microsoft.EntityFrameworkCore;
using myShopAPI.Data;
using NSwag.AspNetCore;
using Microsoft.OpenApi.Models;

namespace myShopAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // ConnectionString
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(); // Doppelt

            builder.Services.AddSwaggerGen(options => // Swagger (NSwag) hinzufügen
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Mein API", Version = "v1" });
            });

            // CORS-Regel hinzufügen (damit Angular API-Aufrufe machen kann)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(); // Doppelt
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meine API v1");
                });
            }

            // WICHTIG: CORS aktivieren
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
