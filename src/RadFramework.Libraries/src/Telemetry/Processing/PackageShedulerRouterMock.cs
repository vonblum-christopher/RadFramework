using RadFramework.Libraries.Telemetry.Channel.Packaging;
using RadFramework.Libraries.Threading.ThreadPools.Queued;

namespace RadFramework.Libraries.Telemetry.Processing
{
    class PackageShedulerRouterMock : ITelemtryPackageThreadShedulerRouter
    {
        private readonly IDelegateSheduler _sheduler;

        public PackageShedulerRouterMock(IDelegateSheduler sheduler)
        {
            _sheduler = sheduler;
        }
        
        public IDelegateSheduler GetShedulerByPackageKind(PackageKind packageKind)
        {
            return _sheduler;
        }
    }
}