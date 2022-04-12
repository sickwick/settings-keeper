using SettingsKeeper.Cache.Extensions;
using SettingsKeeper.Config;
using SettingsKeeper.MongoDb.Extensions;
using SettingsKeeper.Provider.Models;
using SettingsKeeper.RabbitMQ.Extensions;

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

services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
//
// app.UseAuthorization();

app.MapControllers();

app.Run();