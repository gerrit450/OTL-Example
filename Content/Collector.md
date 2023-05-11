# Collector

For this section, I will be talking about the collector and what it is used for in open Telemetry. I will also show the config file used in modifying the collector so that the correct data is received and exported.

# Setting up the Collector

To start setting up the collector, first step is to use docker and pull the image that contains the collector: <br>`docker pull otel/opentelemetry-collector:latest`

now that we have the collector pulled, in a seperate terminal run this container by using the following command: 
`docker run --rm -p 4317:4317 -v ./config.yml:/etc/otel-collector-config.yml otel/opentelemetry-collector:latest`
<br>This command is used to run the container by exposing 4317 port as input and output. Reason this is set to this port number is because the default port number of the collecter is 4317. If this were to change, the uri would have to be changed in the **Program.cs** as well. See the **ZipkinExport** as how this is done 

When you started, you will be greeted with a block of text. This is good as the service is up and running. 
<br>**Note** that the terminal text changes frequently as the collector gathers information all the time.

![Program-services](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/Collector-start.png)


Now, let us send a request to see how the collector would change in response.
I added in a new request in the controller that has a counter as well as Thread.Sleep so we can see if the trace will pick up the duration.

![new code](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/new-request-code.png)


When we hit the request:
![response of new request](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/new-request-response.png)

This is the trace we get now:
![trace on Collector](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/Collector-trace.png)

As you can see, the Collector displays the information of the collector as well as more information on the metrics. I unfortunatly dont have metrics set up correctly but it shows how it is valuable in showing trace information.


# Configuring the Collector


