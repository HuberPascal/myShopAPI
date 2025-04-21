using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AutoMapper;
using myShopAPI.Data;
using myShopAPI.Mappings;


var builder = WebApplication.CreateBuilder(args);

// ---------- CORS ----------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// ---------- DB-Konfiguration ----------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// ---------- AutoMapper ----------
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<CartMappingProfile>();
});
builder.Services.AddSingleton<IMapper>(sp => mapperConfig.CreateMapper());

// ---------- Identity ----------
builder.Services.AddIdentityCore<IdentityUser<Guid>>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// ---------- JWT-Authentifizierung ----------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// ---------- Autorisierung ----------
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder();

// ---------- Swagger ----------
builder.Services.AddEndpointsApiExplorer(); // wichtig für Minimal APIs (optional)
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Mein API", Version = "v1" });

    // Optional: JWT im Swagger aktivieren
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Token (mit 'Bearer ' davor)",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ---------- MVC / API ----------
builder.Services.AddControllers();

// ---------- App-Setup ----------
var app = builder.Build();

// Swagger aktivieren
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Meine API v1");
    });
}

// HTTPS & CORS
app.UseHttpsRedirection();
app.UseCors("AllowLocalhost");

// Auth & AuthZ
app.UseAuthentication();
app.UseAuthorization();

// API-Routing
app.MapControllers();

app.Run();
