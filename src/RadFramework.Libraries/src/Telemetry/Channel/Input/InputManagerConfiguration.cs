namespace RadFramework.Libraries.Telemetry.Channel.Input
{
    public class InputManagerConfiguration
    {
        public Action<byte[]> OnPackageReceived { get; set; }
    }
}