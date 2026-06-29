using System.Text;
using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Middleware;
using BookStore.Presentation.Models;
using BookStore.Presentation.Repositories;
using BookStore.Presentation.Services;
using BookStore.Presentation.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace BookStore.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(
                "logs/log-.txt",
                rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog();

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddScoped<IBooksService, BooksService>();
        builder.Services.AddScoped<IRepository, Repository>();
        builder.Services.AddScoped<IValidator<Book>, BookValidator>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IFilesService, FilesService>();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
            };
        });

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("RequireAdminRole", policy =>
                policy.RequireRole("Admin"))
            .AddPolicy("RequireAuthenticatedUser", policy =>
                policy.RequireAuthenticatedUser());

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token"
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



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
