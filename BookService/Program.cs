using BookService;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Net;

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

builder.Services.AddOpenTelemetry() // add OpenTelemetry

       .WithTracing(tracerProviderBuilder => tracerProviderBuilder // add traces
           .AddSource(DiagnosticsConfig.ServiceName) // add our activity
           .ConfigureResource(resource => resource
           .AddService(DiagnosticsConfig.ServiceName, DiagnosticsConfig.RootServicename) // add our service which is BookService-api
           .AddAttributes(new KeyValuePair<string, object>[]
                {
                    new ("deployment.environment", DiagnosticsConfig.DeploymentEnvironment)
                }))
           .AddAspNetCoreInstrumentation() // allows automatic collection of instrumentation data
          
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

           .AddConsoleExporter() // export telemetry to console
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
