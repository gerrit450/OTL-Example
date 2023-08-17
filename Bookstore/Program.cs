using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

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

string token = File.ReadAllText("../Token.txt");

var ServiceName = builder.Configuration.GetValue<string>("TelemterySettings:Service-name");
var Namespace = builder.Configuration.GetValue<string>("TelemterySettings:Service-namespace");
var Environment = builder.Configuration.GetValue<string>("TelemterySettings:Environment");

builder.Services.AddOpenTelemetry() // add OpenTelemetry

       .WithMetrics(metricsProviderBuilder => metricsProviderBuilder // add metrics
            .AddMeter(ServiceName) // add meter to record metrics
       .ConfigureResource(resource => resource
           .AddService(ServiceName,Namespace)) // add our service name and namespace. In this case, it will be BookstoreApi
           .AddConsoleExporter() // export telemetry to console
       )

       .WithTracing(tracerProviderBuilder => tracerProviderBuilder // add traces
           .AddSource(ServiceName) // add our activity
       .ConfigureResource(resource => resource
           .AddService(ServiceName,Namespace) // add our service which is BookstoreApi
           .AddAttributes(new KeyValuePair<string, object>[]
                {
                    new ("deployment.environment", Environment)
                }))
           .AddAspNetCoreInstrumentation() // allows automatic collection of instrumentation data
           .AddConsoleExporter() // export telemetry to console
           .AddOtlpExporter(otlp =>
           {
               otlp.Endpoint = new Uri("https://ingest.obs-central.platform.myob.com:4318/v1/traces");
               otlp.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.HttpProtobuf;
               otlp.HttpClientFactory = () =>
               {
                   HttpClient client = new HttpClient();
                   client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                   return client;
               };
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
