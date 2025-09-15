
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UserManagement.API.Middleware;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Mapping;
using UserManagement.Application.Shared;
using UserManagement.Application.Validators;
using UserManagement.Infrastructure;
using UserManagement.Infrastructure.Repositories;

namespace UsersAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // ? Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console() // log to console
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // log to files per day
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .CreateLogger();

            builder.Host.UseSerilog(); // replace default logger
            // ------------------------------------------------------
            // 1. Add DbContext (SQL Server using appsettings.json)
            // ------------------------------------------------------
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.Configure<GridSettings>(builder.Configuration.GetSection("GridSettings"));

            // ------------------------------------------------------
            // 2. Add Controllers + FluentValidation
            // ------------------------------------------------------
            builder.Services.AddControllers();
            // Scan whole assembly for validators
            builder.Services.AddFluentValidationAutoValidation()
                            .AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssembly(typeof(UserValidator).Assembly);

            // ------------------------------------------------------
            // 3. Add AutoMapper (register mapping profiles)
            // ------------------------------------------------------
            builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            // ------------------------------------------------------
            // 4. Add Swagger for API testing
            // ------------------------------------------------------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "User Management API",
                    Version = "v1",
                    Description = "API for managing users (Add/Edit/Delete/List)"
                });
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                       .AllowAnyMethod()
                       .AllowAnyHeader();

                });
            });
            var app = builder.Build();

            // ------------------------------------------------------
            // 5. Configure Middleware Pipeline
            // ------------------------------------------------------
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            // ? Add global error handling middleware
            app.UseGlobalExceptionHandler();
            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
