using Librarian.Telemetry;
using OpenTelemetry.Logs;
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

       .WithMetrics(metricsProviderBuilder => metricsProviderBuilder
       .AddMeter(Telemetry.ServiceName)
       .ConfigureResource(resource => resource
           .AddService(Telemetry.ActivitySource.Name))
       .AddConsoleExporter()
       )

       .WithTracing(tracerProviderBuilder => tracerProviderBuilder
       .AddSource(Telemetry.ActivitySource.Name)
       .ConfigureResource(resource => resource
           .AddService(Telemetry.ServiceName))
       .AddAspNetCoreInstrumentation()
       .AddConsoleExporter()
       .AddZipkinExporter(zipkin =>
       {
           zipkin.Endpoint = new Uri("http://127.0.0.1:9411/api/v2/spans");
       }));

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
