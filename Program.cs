using BookStore.Presentation.Interfaces;
using BookStore.Presentation.Middleware;
using BookStore.Presentation.Models;
using BookStore.Presentation.Repositories;
using BookStore.Presentation.Services;
using BookStore.Presentation.Validations;
using FluentValidation;
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

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
