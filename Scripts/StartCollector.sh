#!/bin/bash

# In a new window, run the otel collector on port 4317. [IN-PROGRESS]
#docker run --rm -p 4317:4317 -v config.yaml:/config.yaml otel/opentelemetry-collector-contrib:0.82.0 --config=config.yaml

#docker run --rm -p 4317:4317 -v config.yaml:/etc/otelcol-contrib/config.yaml otel/opentelemetry-collector-contrib:0.82.0

#docker run --rm -p 4317:4317 -v config.yaml:/etc/otelcol-contrib otel/opentelemetry-collector:latest --config=/etc/otelcol-contrib/config.yaml

#docker run --rm -p 4317:4317 -v config.yaml:/etc/otelcol-contrib/config.yaml otel/opentelemetry-collector-contrib:latest

docker run --rm -p 4317:4317 -p 9411 -v config.yaml:/etc/otelcol-contrib otel/opentelemetry-collector-contrib:latest --config=/etc/otelcol-contrib/config.yaml

