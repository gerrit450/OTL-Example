#!/bin/bash

# In a new window, start the zipkin container to listen to 9411

docker run --rm -it -p 9411:9411 --name zipkin openzipkin/zipkin 
