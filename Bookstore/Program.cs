using BookStore.Telemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddOpenTelemetry() // add OpenTelemetry

       .WithMetrics(metricsProviderBuilder => metricsProviderBuilder // add metrics
       .AddMeter(Telemetry.ServiceName) // add meter to record metrics
       .ConfigureResource(resource => resource
           .AddService(Telemetry.ServiceName)) // add our service name. In this case, it will be BookstoreApi
       .AddConsoleExporter() // export telemetry to console
       )

       .WithTracing(tracerProviderBuilder => tracerProviderBuilder // add traces
       .AddSource(Telemetry.ServiceName) // add our activity
       .ConfigureResource(resource => resource
           .AddService(Telemetry.ServiceName)) // add our service which is BookstoreApi
       .AddAspNetCoreInstrumentation() // allows automatic collection of instrumentation data
       .AddConsoleExporter() // export telemetry to console
       .AddZipkinExporter(zipkin => //add zipkins
       {
           zipkin.Endpoint = new Uri("http://127.0.0.1:9411/api/v2/spans"); // Change url
       })
       );


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
