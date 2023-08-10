using System;
using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace Telemetry
{
    public static class OpenTelemetry
    {
        // Creating our span 
        public static Activity StartSpanActivity(string nameOfSpan)
        {
            var activity = new ActivitySource(nameOfSpan);
            var span = activity.StartActivity();

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