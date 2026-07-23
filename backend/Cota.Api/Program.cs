
using Cota.Api;
using Cota.Domain;
using Cota.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITelemetryClient, FakeTelemetryClient>();
builder.Services.AddSingleton<LatestReadingStore>();
builder.Services.AddHostedService<RiverLevelWorker>();

// OpenMeteoClient is registered as a typed client with HttpClientFactory, which provides a pre-configured HttpClient instance for making HTTP requests to the Open-Meteo API. The base address and timeout are set for the HttpClient. 
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<IWeatherClient, OpenMeteoClient>(client =>
{
    client.BaseAddress = new Uri("https://api.open-meteo.com/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Frontend Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("Frontend");

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
