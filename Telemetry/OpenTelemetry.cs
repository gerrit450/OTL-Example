using System;
using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace Telemetry
{
    public static class OpenTelemetry
    {    
        // Creating our activity source
        public static ActivitySource CreateActivitySource(string nameOfSpan)
        {
            return new ActivitySource(nameOfSpan);
        }

        // From that source, create our span
        public static Activity StartSpanActivity(ActivitySource activitySource)
        {
            var span = activitySource.StartActivity();

            return span;
        }

        //creating our metrics meter
        public static Counter<long> CreateMetricCounter(string nameOfMeter)
        {
            Meter Meter = new(nameOfMeter);
            Counter<long> RequestCounter =
            Meter.CreateCounter<long>("app.request_counter");

            return RequestCounter;
        }
    }
}