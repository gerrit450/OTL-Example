using Librarian;
using OpenTelemetry.Exporter;
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

string token = File.ReadAllText("../Token.txt");

builder.Services.AddOpenTelemetry() // add OpenTelemetry

       .WithMetrics(metricsProviderBuilder => metricsProviderBuilder // add metrics
           .AddMeter(DiagnosticsConfig.LibraryMeter.Name) // add meter to record metrics
           .ConfigureResource(resource => resource
               .AddService(DiagnosticsConfig.ServiceName, DiagnosticsConfig.RootServicename)) // add our service name and namespace. In this case, it will be Librarian-api
               .AddConsoleExporter() // export telemetry to console
               .AddOtlpExporter(otlp =>
               {
                   otlp.Endpoint = new Uri("http://otel-collector.platform-enablement.svc:4318");
                   otlp.Protocol = OtlpExportProtocol.HttpProtobuf;
                   otlp.HttpClientFactory = () =>
                   {
                       HttpClient client = new HttpClient();
                       client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                       return client;
                   };
               })
           )

       .WithTracing(tracerProviderBuilder => tracerProviderBuilder // add traces
           .AddSource(DiagnosticsConfig.ServiceName) // add our activity
       .ConfigureResource(resource => resource
           .AddService(DiagnosticsConfig.ServiceName, DiagnosticsConfig.RootServicename) // add our service which is Librarian-api
           .AddAttributes(new KeyValuePair<string, object>[]
                {
                    new ("deployment.environment", DiagnosticsConfig.DeploymentEnvironment)
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
