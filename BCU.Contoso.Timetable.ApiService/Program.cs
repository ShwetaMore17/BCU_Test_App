using BCU.Contoso.Timetable.ApiService.Helpers;
using BCU.Contoso.Timetable.ApiService.Middleware;
using BCU.Contoso.Timetable.Database;
using BCU.Contoso.Timetable.ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using DatabaseDependencies = BCU.Contoso.Timetable.Database.DatabaseDependencies;

namespace BCU.Contoso.Timetable.ApiService;

internal class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add service defaults & Aspire client integrations.
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddProblemDetails();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services
            .AddControllers(options =>
            {
                options.Filters.Add<ExceptionActionFilter>();
            });

        builder.Services.AddSingleton<IRefitHelper, RefitHelper>();

        builder.AddDatabaseDependencies();

        builder.Services.AddCors
        (
            c => c.AddDefaultPolicy
            (
                p =>
                    p
                        .WithOrigins("*")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
            )
        );

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "v1");
            });
        }

        app.MapControllers();

        app.MapDefaultEndpoints();

        app.UseCors();


        var options = app.Services.GetRequiredService<DbContextOptions<TimetableDBContext>>();
        DatabaseDependencies.SetupDatabase(options);

        app.Run();
    }
}