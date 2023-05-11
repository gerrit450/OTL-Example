# Setting up the Client

Now that we have covered the overall system design, let us proceed in gathering the telemetry.
To start off, let us talk about the client. This is where all the information is gathered from and used by the telemetry system.

For telemetry data to be generated, the OpenTelemetry have to be set up in the **Program.cs**. This is used as the starting point of the program and where we can configure the telemetry system.

### Creating the Trace

For us to use a trace, a new one must be created in the Program class as this is the start of our program.
To create our trace, a new ActivitySource have to be created in startup. This is where the start of the activity is recorded and will be further used as part of the builder. In my case, I created a Diagnostics class where expose the service-name and metrics.
<br> In my opinion, I would **highly** recommend you to make a diagnostics class as the **activitySource** would later have to be used to implement additional trace information in the controllers!

![Diagnostics-class](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/DiagnosticsConfig.png)

<br>

### Program-builder


Now that our activitySource is created, we can add it to our program via the use of the builder. Using the builder, we use the AddOpenTelemetry() function where we can create our two types of information, traces and metrics.

For both types of telemetry, we can use lambda methods to add various configurations. There are many important configurations to use. Here I will list the ones I use and what their use are:

* **ConfigureResource**: A method is where we can add all our services. In this case, since there is only one API, this is the only source I added.

* **ASPNetCoreInstrumentation**: This methods manual instrumentation that allows us to add more trace information in different places in the code. Later on, I will show you where I use it in the controllers.
* **AddConsoleExporter**: Use it to export trace information to the console. Very easy to set-up and allows instant display of tracing information.

* **AddOtlpExporter**: Our Collector where the trace information is exported. Trace information is displayed in another console. Default port is **4317** but can be changed.

* **AddZipkinExporter**: Zipkin visual interpretor where our trace information is displayed on a website. Shows span and trace information. Default port is **9411** but can be changed. I added an example in the Program.cs on how to edit the endpoint.

For **metrics**, I didnt add much as I was focused on traces but if you want more information, here is a great guide: [link](https://opentelemetry.io/docs/instrumentation/net/getting-started/#metrics)

Code:
![Program-services](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/Program-services.png)

**NOTE:** When adding **OpenTelemetry** to your own ASP.NET project, packages have to be added via using Nuget. See this guide here: [OpenTelemetry-C#](https://opentelemetry.io/docs/instrumentation/net/getting-started/)

<br>

### Displaying the trace on console

After setting up the trace in Program.cs, you should be able to view it as soon as you send a request. Remember how we added the **AddConsoleExporter** in the Program? This will allow us to see it in the console! We also have other exporters that I will cover later on :)

First, let us start up our api:

![starting-up](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/starting-up.png)

Now that it is running, let us go to http://localhost:5085/swagger/index.html and send a request!

![Sending-request](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/API-Get-request.png)

After we send the request, we can now see it on the console:
![console-display](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/console-display.png)

As you can see, the console displays information all about our trace such as hostname, method used and our message that we entered when we sent the request!
This information is generated for every request you sent and is the baseline of our trace.


### Adding trace via Manual Instrumentation

Now that we have it displayed and working, we can now try and add more information to our traces.

Let us go to our **WeatherForecastController** and modifying one of the requests:

![adding trace activity](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/Manual-Instrumentation.png)

Here I created a GET request that I made it fail on purpose so that we get error messages in our trace. I also added in a **Thread.Sleep** so that we can measure the duration later on when we do the **zipkin**.
<br>To modify our existing trace, we use the same DiagnosticsConfig class as created in the Program.cs. 
<br>This allows us to use **activitySource** class which will expose the  **StartActivity** method to start recording the activity of this request.

below that line, I also added in a **tag** which are tags associated with the trace. I added in a GUID so we can easily see the trace :)


Now, we can hit this endpoint and see the result:

![Get-bad-request](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/Get-bad-request.png)


Now, let us look at our console:
![Get-bad-request-trace](https://github.com/gerrit450/OTL-Example/blob/Demo/Images/bad-request-trace.png)

As you can see, here we can view the trace with the message including the GUID as well as the 400 request it send back. This shows how manual instrumentation can be used to attach messages to traces. I see this as a great tool to attach messages when exceptions are thrown.

