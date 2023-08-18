using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Librarian
{
    /*
     * This needs to be unique for each service.
     */
    public static class DiagnosticsConfig
    {
        public const string RootServicename = "otl-example";
        public const string ServiceName = "librarian-service";
        public const string DeploymentEnvironment = "Development";
        public static readonly ActivitySource ActivitySource = new("librarian-service");
        public static Meter LibraryMeter = new("librarymeter");

    }
}