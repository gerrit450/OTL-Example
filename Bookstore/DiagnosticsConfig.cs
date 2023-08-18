using System.Diagnostics;

public static class DiagnosticsConfig
{
    public const string RootServicename = "bookstore-service";
    public const string ServiceName = "bookstore-demo";
    public const string DeploymentEnvironment = "Development";
    public static readonly ActivitySource ActivitySource = new("bookstore-service");
}
