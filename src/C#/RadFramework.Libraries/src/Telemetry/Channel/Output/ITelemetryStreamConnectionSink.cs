namespace RadFramework.Libraries.Telemetry.Channel.Output
{
    public interface ITelemetryStreamConnectionSink : IDisposable
    {
        void SendPackage(byte[] package);
    }
}