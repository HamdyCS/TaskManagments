using Api.ExceptionHandler;
using Application;
using Infrastructure;
using Serilog;
using Serilog.Enrichers.Span;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//add serilog
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.WithSpan();
});

//app services
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();



var app = builder.Build();


app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
