using RadFramework.Libraries.Telemetry.Channel.Packaging;

namespace RadFramework.Libraries.Telemetry.Channel
{
    public delegate object ProcessTelemetryRequest(PackageKind packageKind, object payload);
}