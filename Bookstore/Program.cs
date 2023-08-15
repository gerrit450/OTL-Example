using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*
* In here, this is where our Trace and metrics are created. From here, we can configure our telemetry by adding our service
* as well as configure where to export our telemetry information. 
* To add spans to our trace, please see the controller example.
* For more information on OpenTelemetry, please see this guide: https://opentelemetry.io/docs/instrumentation/net/getting-started/
*/

builder.Services.AddOpenTelemetry() // add OpenTelemetry

       .WithMetrics(metricsProviderBuilder => metricsProviderBuilder // add metrics
            .AddMeter("Bookstore") // add meter to record metrics
       .ConfigureResource(resource => resource
           .AddService("Bookstore")) // add our service name. In this case, it will be BookstoreApi
           .AddConsoleExporter() // export telemetry to console
       )

       .WithTracing(tracerProviderBuilder => tracerProviderBuilder // add traces
           .AddSource("Bookstore") // add our activity
       .ConfigureResource(resource => resource
           .AddService("Bookstore")) // add our service which is BookstoreApi
           .AddAspNetCoreInstrumentation() // allows automatic collection of instrumentation data
           .AddConsoleExporter() // export telemetry to console
           .AddOtlpExporter(otlp =>
           {
               otlp.Endpoint = new Uri("http://localhost:4317");
               otlp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
           })
           //.AddZipkinExporter(zipkin => //add zipkins
           //{
           // zipkin.Endpoint = new Uri("http://127.0.0.1:9411/api/v2/spans"); // Change url
           // })
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
