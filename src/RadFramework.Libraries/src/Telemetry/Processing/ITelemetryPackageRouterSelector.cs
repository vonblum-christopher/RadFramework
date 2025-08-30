using RadFramework.Libraries.Telemetry.Channel.Packaging;

namespace RadFramework.Libraries.Telemetry.Processing
{
    public interface ITelemetryPackageRouterSelector
    {
        Func<(PackageKind packageType, object payload), object> GetPackageKindRouterDelegate(PackageKind packageKind);
    }
}