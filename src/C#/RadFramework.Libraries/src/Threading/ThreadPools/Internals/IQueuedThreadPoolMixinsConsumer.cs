using System.Collections.Concurrent;
using RadFramework.Libraries.DataTypes;

namespace RadFramework.Libraries.Threading.ThreadPools.Internals
{
    public interface IQueuedThreadPoolMixinsConsumer<TQueueTask> : IThreadPoolMixinsConsumer
    {
        CounterSemaphore ProcessIncomingWorkSemaphore { get; }
        ConcurrentQueue<TQueueTask> Queue { get; }
        Action<TQueueTask> ProcessWorkloadDelegate { get; }
    }
}