namespace RadFramework.Libraries.Telemetry.Channel.Input
{
    public interface ITelemetryStreamConnectionSource  : IDisposable
    {
        Action<byte[]> OnPackageReceived { get; set; }
    }
}