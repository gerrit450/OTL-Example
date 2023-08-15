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
         
builder.Services.AddOpenTelemetry() // add OpenTelemetry

       .WithMetrics(metricsProviderBuilder => metricsProviderBuilder // add metrics
            .AddMeter("Librarian") // add meter to record metrics
       .ConfigureResource(resource => resource
           .AddService("Librarian")) // add our service name. In this case, it will be Librarian
           .AddConsoleExporter() // export telemetry to console
       )

       .WithTracing(tracerProviderBuilder => tracerProviderBuilder // add traces
            .AddSource("librarian") // add our activity
       .ConfigureResource(resource => resource
           .AddService("Librarian")) // add our service using our service name
           .AddAspNetCoreInstrumentation() // allows automatic collection of instrumentation data
           .AddConsoleExporter() // export telemetry to console
           .AddOtlpExporter(otlp =>
           {
               otlp.Endpoint = new Uri("http://localhost:4317");
               otlp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
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
