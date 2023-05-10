#!/bin/bash

#Step 0 Change the Program.cs startup to export 
# Add .AddConsoleExporter() to send telemetry to the ASP.NET application Console
# Add .AddOtlpExporter() to send OTLP telemetry (to localhost:4317) 
# .AddZipkinExporter(o =>
#            {
#               o.Endpoint = new Uri("http://0.0.0.0:9411/api/v2/spans");
#            })
# ... to send zipkin format telemetry (to localhost:9411)

# Step 1 run the app
dotnet run

#Step 2 In a new window, run the otel collector on port 4317
docker run --rm -p 4317:4317 -p 9411:9411 -v ./config.yaml:/etc/otelcol-contrib/config.yaml otel/opentelemetry-collector:latest

#Step 3 In a new window, start the zipkin container to listen to 9411 (This will also a ui on localhost:9411)
docker run --rm -d -p 9411:9411 --name zipkin openzipkin/zipkin
