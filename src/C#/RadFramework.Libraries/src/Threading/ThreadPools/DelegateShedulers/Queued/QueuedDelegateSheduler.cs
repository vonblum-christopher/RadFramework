using RadFramework.Libraries.Threading.Internals;
using RadFramework.Libraries.Threading.ThreadPools.Queued;
using RadFramework.Libraries.Web;

namespace RadFramework.Libraries.Threading.ThreadPools.DelegateShedulers.Queued
{
    /// <summary>
    /// A thread pool featuring a queue for workloads.
    /// </summary>
    public class QueuedDelegateSheduler : QueuedThreadPool<Action>, IDelegateSheduler
    {
        public QueuedDelegateSheduler(Action<Action, PoolThread> processingWorkloadYieldedError,
            Action<Action> processWorkloadDelegate,
            int processingThreadAmount,
            ThreadPriority processingThreadPriority,
            Action processingDelegate,
            string threadDescription = null) 
            : base(processingWorkloadYieldedError,
                processWorkloadDelegate,
                processingThreadAmount,
                processingThreadPriority,
                processingDelegate,
                threadDescription)
        {
        }

        public void Enqueue(Action task)
        {
            QueuedThreadPoolMixins.Enqueue(this, task);
        }

        public void Enqueue(IEnumerable<Action> tasks)
        {
            QueuedThreadPoolMixins.Enqueue(this, tasks);
        }
    }
}