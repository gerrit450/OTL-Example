using System.Diagnostics;
using System.Net;

namespace Bookstore
{
    /*
     * This needs to be unique for each service.
     */
    public static class DiagnosticsConfig
    {
        public static readonly string RootServicename = "otl-example";
        public static readonly string ServiceName = "bookstore";
        public static readonly string DeploymentEnvironment = "Development";
        public static readonly string Host = Dns.GetHostName();
        public static readonly ActivitySource ActivitySource = new(ServiceName);
    }
}