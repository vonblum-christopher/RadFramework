using RadFramework.Libraries.Serialization;
using RadFramework.Libraries.Telemetry.Channel.Input;
using RadFramework.Libraries.Telemetry.Channel.Output;
using RadFramework.Libraries.Telemetry.Channel.Packaging;
using RadFramework.Libraries.Telemetry.Processing;
using RadFramework.Libraries.Threading.ThreadPools.DelegateShedulers;
using RadFramework.Libraries.Threading.ThreadPools.DelegateShedulers.Queued;

namespace RadFramework.Libraries.Telemetry.Channel
{
    /// <summary>
    /// 
    /// </summary>
    public class LightweightTelemetryChannel : TelemetryChannelBase
    {
        public LightweightTelemetryChannel(
            IContractSerializer contractSerializer,
            ProcessTelemetryRequest executeRequestDelegate,
            ProcessTelemetryEvent handleEventDelegate,
            ITelemetryStreamConnectionSource connectionSource,
            ITelemetryStreamConnectionSink connectionSink)
            : base(
                contractSerializer,
                connectionSource, 
                connectionSink,
                new PackageProcessingDelegateRouter(new Dictionary<PackageKind, Func<(PackageKind packageKind, object payload), object>>
                {
                    {PackageKind.Request, tuple => executeRequestDelegate(tuple.packageKind, tuple.payload)},
                    {PackageKind.Event, tuple => handleEventDelegate(tuple.packageKind, tuple.payload)}
                }), 
                new PackageShedulerRouterMock(new StayInCurrentThreadDelegateSheduler()),
                new TelemetryPackageWrapper(contractSerializer),
                new QueuedDelegateShedulerWithLongRunningOperationsDispatchCapabilities(250, ThreadPriority.Highest),
                new QueuedDelegateShedulerWithLongRunningOperationsDispatchCapabilities(250, ThreadPriority.Highest))
        {
        }
    }
}