using SettingsKeeper.Client.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureAppConfiguration((context, config) =>
{
    config.AddJsonFile("settings.json", false, true);
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();