using RadFramework.Libraries.Telemetry.Channel.Packaging;
using RadFramework.Libraries.Threading.ThreadPools.Queued;

namespace RadFramework.Libraries.Telemetry.Processing
{
    public interface ITelemtryPackageThreadShedulerRouter
    {
        IDelegateSheduler GetShedulerByPackageKind(PackageKind packageKind);
    }
}