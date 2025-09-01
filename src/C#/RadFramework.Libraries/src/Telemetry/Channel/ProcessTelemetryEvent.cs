using RadFramework.Libraries.Telemetry.Channel.Packaging;

namespace RadFramework.Libraries.Telemetry.Channel
{
    public delegate object ProcessTelemetryEvent(PackageKind packageKind, object payload);
}