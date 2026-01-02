using AutoMapper;
using Domain;
using Infraestructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Model;
using Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ===== CONFIGURACI�N DE SERVICIOS =====

// 1. DbContext con MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString,
        new MySqlServerVersion(new Version(8, 0, 21)))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
);

// 2. AutoMapper - Configuraci�n Manual
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfiles>());

// 2.5. HttpContextAccessor (para ClaimsHelper)
builder.Services.AddHttpContextAccessor();

// 3. Inyecci�n de Dependencias (ServiceRegistrations)
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddDomainServices();
builder.Services.AddPersistenceServices();

// 4. Autenticaci�n JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// 5. CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// 6. Controllers
builder.Services.AddControllers();

// 7. Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Mi API",
        Version = "v1"
    });

    // Configuraci�n JWT en Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ===== CONFIGURACI�N DEL PIPELINE HTTP =====

var app = builder.Build();

// Swagger (solo en desarrollo)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS
app.UseCors();

// HTTPS Redirection
app.UseHttpsRedirection();

// Autenticaci�n y Autorizaci�n
app.UseAuthentication();
app.UseAuthorization();

// Mapear Controllers
app.MapControllers();

app.Run();
