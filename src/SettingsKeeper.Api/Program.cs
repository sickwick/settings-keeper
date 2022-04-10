using SettingsKeeper.Cache.Extensions;
using SettingsKeeper.Config;
using SettingsKeeper.MongoDb.Extensions;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;
IServiceCollection services = builder.Services;
// Add services to the container.
services.AddOptions();
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSettingsKeeperCache(configuration);
services.AddMongoDb(configuration);

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