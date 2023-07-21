#!/bin/bash

# In a new window, run the otel collector on port 4317. [IN-PROGRESS]
#docker run --rm -p 4317:4317 -v  ./Telemetry/Collector/otel-collector-config.yml otel/opentelemetry-collector:latest