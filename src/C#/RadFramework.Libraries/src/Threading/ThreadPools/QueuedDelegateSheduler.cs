using RadFramework.Libraries.Threading.Interfaces;
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
        public QueuedDelegateSheduler(
                int threadAmountPerCore,
                ThreadPriority priority,
                OnWorkloadArrivedDelegate<Action> onWorkloadArrivedDelegate,
                OnProcessingError<Action> onProcessingError,
                string threadDescription = null) : 
                    base(threadAmountPerCore,
                            priority,
                            onWorkloadArrivedDelegate,
                            onProcessingError,
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