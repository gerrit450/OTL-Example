using System.Diagnostics;

namespace BookService
{
    /*
     This needs to be unique for each service.
     */
    public static class DiagnosticsConfig
    {
        public const string RootServicename = "otl-example";
        public const string ServiceName = "bookservice";
        public const string DeploymentEnvironment = "Development";
        public static readonly ActivitySource ActivitySource = new(ServiceName);
    }
}