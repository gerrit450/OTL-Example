using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace Librarian.Telemetry
{
    public static class Telemetry
    {
        // Creating the Trace source
        public const string ServiceName = "Librarian";
        public static ActivitySource ActivitySource = new ActivitySource(ServiceName);

        // Creating the Metrics source
        public static Meter Meter = new(ServiceName);
        public static Counter<long> RequestCounter =
            Meter.CreateCounter<long>("app.request_counter");
    }
}