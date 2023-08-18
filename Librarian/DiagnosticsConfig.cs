using System.Diagnostics;

namespace Librarian
{
    public static class DiagnosticsConfig
    {
        public const string RootServicename = "librarian-service";
        public const string ServiceName = "bookstore-demo";
        public const string DeploymentEnvironment = "Development";
        public static readonly ActivitySource ActivitySource = new("librarian-service");

    }
}