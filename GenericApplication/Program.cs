using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApplication1.Data.Interfaces;
using WebApplication1.Data.Repositories;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;
using MiniMarket_API.Model.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuring Swagger/OpenAPI for API documentation and testing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    setupAction.AddSecurityDefinition("MiniMarketApiBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Paste Token here after login."
    });

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "MiniMarketApiBearerAuth"
                }
            }, new List<string>()
        }
    });
});

// Configuring DbContext with SQL Server connection string
builder.Services.AddDbContext<GenericAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GenericConnectionString"))
);

// Registering the UserService and PasswordHasher for dependency injection
builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();

// Configuring CORS policies
#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("UnsafeAllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("http://localhost", "http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
#endregion

// Configuring JWT Authentication
#region Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };
    })
    .AddJwtBearer("PasswordRecovery", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Recovery:Issuer"],
            ValidAudience = builder.Configuration["Recovery:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Recovery:SecretForKey"]))
        };
    });
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
