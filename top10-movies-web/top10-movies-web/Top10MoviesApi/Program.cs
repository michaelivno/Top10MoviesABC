using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Top10MoviesApi.Data;
using Top10MoviesApi.Services;
using AutoMapper;
using Top10MoviesApi.Mappings;
using Top10MoviesApi;
using Top10MoviesApi.DbAction;


var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    logger.Debug("----------------------------");
    logger.Debug("--- Start Top10MoviesApi ---");
    logger.Debug("----------------------------");
} catch (Exception e)
{
    logger.Error($"Log Error --- {e.Message}");
}

var builder = WebApplication.CreateBuilder(args);

// Configure NLog
builder.Services.AddLogging();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Host.UseNLog();



// Add services to the container.
var services = builder.Services;
var configuration = builder.Configuration;


services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

services.AddAutoMapper(typeof(MovieMapperProfile));
services.AddSingleton<IDatabaseConfig>(sp => new DatabaseConfig
{
    ConnectionString = configuration.GetConnectionString("DefaultConnection")
});
services.AddSingleton<DatabaseInitializer>();
services.AddScoped<IMovieDbAction, MovieDbAction>();
services.AddScoped<IMovieService, MovieService>();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Top10Movies API", Version = "v1" });
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var servicesProvider = scope.ServiceProvider;
    var dbInitializer = servicesProvider.GetRequiredService<DatabaseInitializer>();
    await dbInitializer.InitializeDatabaseAsync();
}
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Top10Movies API V1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseCors("AllowReactApp");

app.MapControllers();

app.Run();
