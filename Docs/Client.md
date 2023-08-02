# Client side

As shown earlier, the client side is composed of multiple services. The main one being **BookStore** that is mainly interacted with. 
The reason I have created multiple clients was to showcase **distributed** tracing over various systems. 
A distributed tracing meaning a series of operations that spans multiple services.
Note that for each service, telemetry should be added to the Program.cs so that automatic telemetry is recorded for each individual service.

## Setting up OpenTelemetry

To set up OpenTelemetry in your own ASP.net core project, you will need to first create the telemetry class. This class is a model that is used to create the trace and metrics.
Remember to add in your service name!
```csharp
using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace OurService.Telemetry
{
    public static class Telemetry
    {
        // Creating the Trace source
        public const string ServiceName = "serviceName"; // your service-name
        public static ActivitySource ActivitySource = new ActivitySource(ServiceName); // our trace source

        // Creating the Metrics source
        public static Meter Meter = new(ServiceName); // our metrics meter
        public static Counter<long> RequestCounter =
            Meter.CreateCounter<long>("app.request_counter"); // request counter
    }
}
```

Once we have created our class, we can continue by installing some packages. These packages are for adding the OpenTelemetry to your project including Zipkins.

In your terminal, in the same directory as the **csproj** file, run these commands:

`dotnet add package OpenTelemetry.Exporter.Console`<br>
`dotnet add package OpenTelemetry.Exporter.Zipkin` <br>
`dotnet add package OpenTelemetry.Extensions.Hosting`<br>
`dotnet add package OpenTelemetry.Instrumentation.AspNetCore --prerelease`<br>

From here, we can now add it to our project through the use of the **Program.cs** class.

In here, we can add in our OpenTelemetry using the foloowing code.
```csharp

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
           })
        );
```

What each line does is the following:

**Trace:**
- `WithTracing()` - The start of our trace configuration. Using a lambda, we can add various configurations to our trace.
    - `AddSource()` - Adds a source to our trace. We just need to add in a name to create it.
- `ConfigureResource()` - Where we configure our resources. From here we can add our exporters and services.
    - `AddService()` - Adds a service to our trace. A service is the representation of our service, for example, in this example, we have the bookstore service.
    - `AddAspNetCoreInstrumentation()` - Enables automatic collection of ASP.NET core data. Can be removed if not useful.
    - `AddConsoleExporter()` - Export our telemetry to the console.
    - `AddZipkinExporter()` - Export our telemetry to zipkins.

**Metrics:**
- `WithMetrics()` - The start of our metrics configuration. Using a lambda, we can add various configurations to our metrics.
    - `AddMeter()` - Adds a meter to the metrics configuration. This uses the Meter atrribute in the Telemetry class.
- `ConfigureResource()` - Where we configure our resources. From here we can add our exporters and services.
    - `AddService()` - Adds a service to our metrics.
    - `AddConsoleExporter()` - Export our metric data to the console.

<br>

## Using Telemetry

Now that we have the telemtry trace set up, all we need to do is just run the application like normal. 
It will look something like this:
![Running-telemetry](https://github.com/gerrit450/OTL-Example/blob/Demo/Docs/Images/DotnetRunTelemetry.png)










