namespace RadFramework.Libraries.Telemetry.Channel.Packaging
{
    public interface IPayloadPackage
    {
        PackageKind PackageKind { get; }
        string PayloadType { get; }
        byte[] Payload { get; }
    }
}