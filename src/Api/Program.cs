using Api.ExceptionHandler;
using Api.Hubs.Notification;
using Application;
using Application.Common.Interfaces.Services;
using Infrastructure;
using Serilog;
using Serilog.Enrichers.Span;
using System.Text.Json.Serialization;

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

//Configure enum serialization as strings

//For minimal api
builder.Services.ConfigureHttpJsonOptions(opt =>
{
    opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
//For controllers
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter());
});


//add signalR
builder.Services.AddSignalR();

//add hub services
builder.Services.AddScoped<INotificationHubService, NotificationHubService>();


//app services
builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration);

//add policies
builder.Services.AddPolicies();


builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();



var app = builder.Build();


app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//map hub
app.MapHub<NotificationHub>("/notificationHub");

app.Run();
