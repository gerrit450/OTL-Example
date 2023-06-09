# OpenTelemetry Demo

Hi there, welcome to my OpenTelemetry project! For this demo, I will be showing how openTelemetry works on a local environment. Please note that this is only a starting guide as I am still learning the system! Any new discoveries I make, will be added to this guide :)


### Design

To start off, is it important to discuss the architecture design of this system.
when I designed the system, I intended to keep the design small and simple to demonstrate great OpenTelemetry is in creating traces.

For openTelemetry to get information, it is important to have a source, in our case, it will be a client that will be a ASP.NET API that will be used as our sample API.

Next. there is a OpenTelemetry collector which is used to collect and redistribute trace information. Collectors are used as a mediatory between the client and the visual exporter and can also process information so only important trace information is sent.

Finally, there is zipkin. This is a *visual* interpretor of trace data and is used to display trace informatio such as request sent, service that it is sent from and so forth. There are many visual interpretors, zipkins is one of many interpretors.
If you are curious about what other options there is, here is a great link on more exporters: [link](https://opentelemetry.io/docs/instrumentation/net/exporters/)

![Architechture design](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/Architechtural-design.png)

<br>

To find more about each individual system, I have created a Table of contents for view of each system:
<br>

### Table of Contents:
<br>

[1. Client-side](https://github.com/gerrit450/OTL-Example/blob/Demo/Content/ASP.NET-Core.md)

[2. Collector](https://github.com/gerrit450/OTL-Example/blob/Demo/Content/Collector.md)

[3. Zipkins](https://github.com/gerrit450/OTL-Example/blob/Demo/Content/Zipkins.md)


