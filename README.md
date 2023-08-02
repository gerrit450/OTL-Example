# OpenTelemetry Demo

Hi there, welcome to my OpenTelemetry project! For this demo, I will be showing how openTelemetry works on a local environment. Please note that this is only a starting guide as I am still learning the system! Any new discoveries I make, will be added to this guide :)


### Design

To start off, is it important to discuss the architecture design of this system.
when I designed the system, I intended to keep the design small and simple to demonstrate great OpenTelemetry is in creating traces.

For this demonstration, I will be covering the client and a visual interpretor where we will be exporting our telemetry to..

For openTelemetry to get information, it is important to have a source, in our case, we will be using the bookstore as our client.

Finally, there is zipkin. This is a *visual* interpretor that is used to display trace information including spans. There are many visual interpretors with Zipkins as one such example.
If you are curious about what other options there is, here is a great link on more exporters: [link](https://opentelemetry.io/docs/instrumentation/net/exporters/)

Here is the design for this sample:

![Architechture design](https://github.com/gerrit450/OTL-Example/blob/Demo/Docs/Images/Design.png)


<br>

To find more about each individual system, I have created a Table of contents for view of each system:

### Table of Contents:
<br>

[1. Client-side](https://github.com/gerrit450/OTL-Example/blob/Demo/Docs/Client.md)

[2. Zipkins](https://github.com/gerrit450/OTL-Example/blob/Demo/Docs/Zipkins.md)

<br>

**Note**: I am still in progress in understanding the collector. When I understand more, I will include it here.