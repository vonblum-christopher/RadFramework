namespace RadFramework.Libraries.Telemetry.Channel.Packaging
{
    [MessagePackObject()]
    public class NestedPackage
    {
        [Key(0)]
        public string PayloadType { get; set; }
        [Key(1)]
        public byte[] Payload { get; set; }
    }
}