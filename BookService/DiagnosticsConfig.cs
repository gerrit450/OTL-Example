using System.Diagnostics;

namespace BookService
{
    /*
     This needs to be unique for each service.
     */
    public static class DiagnosticsConfig
    {
        public const string RootServicename = "Service-name";
        public const string ServiceName = "bookstore-demo";
        public const string DeploymentEnvironment = "Development";
        public static readonly ActivitySource ActivitySource = new("bookservice");
    }
}