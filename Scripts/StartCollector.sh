#!/bin/bash

# In a new window, run the otel collector on port 4317. [IN-PROGRESS]
docker run -p 4317:4317 -v config.yaml:/etc/otelcol-contrib/config.yaml otel/opentelemetry-collector:latest