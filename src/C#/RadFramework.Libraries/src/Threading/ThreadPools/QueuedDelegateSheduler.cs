using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Threading.Interface;
using RadFramework.Libraries.Threading.ThreadPools.Internals;

namespace RadFramework.Libraries.Threading.ThreadPools
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