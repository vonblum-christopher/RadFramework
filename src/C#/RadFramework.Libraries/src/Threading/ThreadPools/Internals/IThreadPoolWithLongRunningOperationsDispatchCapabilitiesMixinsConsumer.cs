using RadFramework.Libraries.DataTypes;

namespace RadFramework.Libraries.Threading.ThreadPools.Internals
{
    public interface IThreadPoolWithLongRunningOperationsDispatchCapabilitiesMixinsConsumer : IThreadPoolMixinsConsumer
    {
        Action<PoolThread> OnShiftedToLongRunningOperationsPool { get; }
        int LongRunningThreadDispatchTimeout { get; }
        int LongRunningOperationCancellationTimeout { get; }
        int LongRunningOperationLimit { get; }
        ObjectReferenceRegistry<PoolThread> LongRunningOperationsRegistry { get; }
        ThreadPriority LongRunningOperationThreadsPriority { get; }
    }
}