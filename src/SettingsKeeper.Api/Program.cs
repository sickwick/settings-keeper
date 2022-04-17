using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using SettingsKeeper.Api.Models;
using SettingsKeeper.Cache.Extensions;
using SettingsKeeper.Config;
using SettingsKeeper.MongoDb.Extensions;
using SettingsKeeper.Provider.Models;
using SettingsKeeper.RabbitMQ.Extensions;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
IServiceCollection services = builder.Services;
// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
services.AddOptions();
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.Configure<SettingsKeeperSettings>(configuration.GetSection("SettingsKeeper"));
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSettingsKeeperCache(configuration);
services.AddMongoDb(configuration);
services.AddRabbitMq(configuration);
services.AddHttpContextAccessor();

services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler((options) =>
{
    options.Run(async context =>
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = MediaTypeNames.Application.Json;
        var ex = context.Features.Get<IExceptionHandlerFeature>();
        if (ex != null)
        {
            var err = new ErrorViewModel()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = ex.Error.Message
            };
            var respose = JsonSerializer.Serialize(err);
            await context.Response.WriteAsync(respose).ConfigureAwait(false);
        }
    });
});
// app.UseHttpsRedirection();
//
// app.UseAuthorization();

app.MapControllers();

app.Run();