# OpenTelemetry Demo

Hi there, welcome to my OpenTelemetry project! For this demo, I will be showing how openTelemetry works on a local environment.

To start off, is it important to discuss the architecture design of this system. For this syste to work, it relies on having an ASP.NET client which is a simple API for this demonstrating.

Next. there is a OpentTelemetry collector which is used to collect and redistribute trace information. Collectors are used as a mediatory between the client and the visual exporter and can process and information so only imporant trace information is sent.

Finally, there is zipkin. This is a visual interpretor of trace data and is used to display trace informatio such as request sent, service that it is sent from and so forth. There are many visual interpretors, zipkins is one of many interpretors.
If you are curious about what other options there is, here is a great link on more exporters: [exporters](https://opentelemetry.io/docs/instrumentation/net/exporters/)

![Architechture design](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/Architechtural-design.png%20=design.png)


Now that we have covered the 