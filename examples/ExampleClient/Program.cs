using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using SettingsKeeper.Api.Models;
using SettingsKeeper.Client.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureAppConfiguration((context, config) =>
{
    config.AddSettingsFile("settings.json",  reloadOnChange: true);
    config.Build();
});

// Add services to the container.
IConfiguration configuration = builder.Configuration;
var rv = configuration.AsEnumerable().ToList();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSettingsKeeperClient(configuration);

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

// app.UseAuthorization();

app.MapControllers();

app.Run();