using BookService.Telemetry;
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

/*
* In here, this is where our Trace and metrics are created. From here, we can configure our telemetry by adding our service
* as well as configure where to export our telemetry information. 
* To add spans to our trace, please see the controller example.
* For more information on OpenTelemetry, please see this guide: https://opentelemetry.io/docs/instrumentation/net/getting-started/
*/

builder.Services.AddOpenTelemetry()

       .WithMetrics(metricsProviderBuilder => metricsProviderBuilder
       .AddMeter(Telemetry.ServiceName)
       .ConfigureResource(resource => resource
           .AddService(Telemetry.ServiceName))
       .AddConsoleExporter()
       )

       .WithTracing(tracerProviderBuilder => tracerProviderBuilder
       .AddSource(Telemetry.ServiceName)
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
