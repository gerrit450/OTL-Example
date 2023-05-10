using System.Diagnostics;
using System.Diagnostics.Metrics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()

    .WithTracing(tracerProviderBuilder =>
        tracerProviderBuilder
            .AddSource(DiagnosticsConfig.ActivitySource.Name)
            .ConfigureResource(resource => resource
                .AddService(DiagnosticsConfig.ServiceName))
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter()
            .AddOtlpExporter() // Default http://localhost:4317
            .AddZipkinExporter(o =>
            {
                o.Endpoint = new Uri("http://0.0.0.0:9411/api/v2/spans");
            })
            );
            

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

public static class DiagnosticsConfig
{
    // Creating Trace source
    public const string ServiceName = "myService";
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);

    // Creating Metrics source
    public static Meter Meter = new(ServiceName);
    public static Counter<long> RequestCounter =
        Meter.CreateCounter<long>("app.request_counter");
}

