
using Cota.Api;
using Cota.Domain;
using Cota.Infrastructure;
using Cota.Infrastructure.Ana;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the AnaTelemetryClient as a singleton service, which will be used to interact with the ANA API for telemetry data. The client is configured with an HttpClient that has a base address and timeout set for making requests to the ANA API.
// builder.Services.AddSingleton<ITelemetryClient, FakeTelemetryClient>(); 
builder.Services.AddHttpClient<ITelemetryClient, AnaTelemetryClient>(client =>
{
    client.BaseAddress = new Uri("https://www.ana.gov.br/hidrowebservice/");
    client.Timeout = TimeSpan.FromSeconds(30);
});

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

// Ana Options
builder.Services.Configure<AnaOptions>(builder.Configuration.GetSection(AnaOptions.SectionName));

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
