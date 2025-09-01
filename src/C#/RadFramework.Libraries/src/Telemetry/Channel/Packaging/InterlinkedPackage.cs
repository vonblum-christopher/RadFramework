namespace RadFramework.Libraries.Telemetry.Channel.Packaging
{
    [MessagePackObject]
    public class InterlinkedPackage : NestedPackage
    {
        [Key(2)]
        public Guid Id { get; set; }
    }
}