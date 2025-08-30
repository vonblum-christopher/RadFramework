namespace RadFramework.Libraries.Telemetry.Channel.Packaging
{
    [MessagePackObject]
    public class PayloadPackage : NestedPackage, IPayloadPackage
    {
        [Key(2)]
        public PackageKind PackageKind { get; set; }
    }
}