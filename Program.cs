
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using study.db;
using study.endpoints;

var builder = WebApplication.CreateSlimBuilder(args);

// Configurar el DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
  options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptionsAction: sqlOptions =>
    {
      sqlOptions.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(30),
        errorNumbersToAdd: null
      );
    }
  )
);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Key"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
{
  throw new Exception("JwtSettings is not configured correctly");
}

// esto es para que se pueda usar el JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = issuer,
      ValidAudience = audience,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
  });

builder.Services.AddAuthorizationBuilder()
  .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))
  .AddPolicy("StudentOnly", policy => policy.RequireRole("Student"))
  .AddPolicy("AdminOrStudent", policy => policy.RequireRole("Admin", "Student"));

// esto es para que se pueda usar el endpoint
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    In = ParameterLocation.Header,
    Description = "Ingrese el token JWT como: Bearer {token}",
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey
  });

  options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// esto es para que se pueda usar el controlador
builder.Services.AddControllers();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

// esto es para que se autentique el usuario
app.UseAuthentication();

// esto es para que se autorice el usuario
app.UseAuthorization();

app.Logger.LogInformation($"App is running on: {app.Urls.FirstOrDefault() ?? "http://localhost:5000"}");

AuthEndpoints.Map(app);
RoleEndpoints.Map(app);
StudentEndpoints.Map(app);

app.Run();