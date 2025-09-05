using RadFramework.Libraries.Abstractions;
using RadFramework.Libraries.Threading.Interface;
using RadFramework.Libraries.Threading.ThreadPools.Internals;

namespace RadFramework.Libraries.Threading.ThreadPools
{
    public class QueuedDelegateShedulerWithLongRunningOperationsDispatchCapabilities : QueuedThreadPoolWithLongRunningOperationsDispatchCapabilities<Action>, IDelegateSheduler
    {
        public QueuedDelegateShedulerWithLongRunningOperationsDispatchCapabilities(int processingThreadAmount,
            ThreadPriority processingThreadPriority,
            OnWorkloadArrivedDelegate<Action> processingMethod,
            OnProcessingError<Action> onProcessingError,
            int dispatchLongRunningThreadTimeout,
            ThreadPriority longRunningOperationThreadsPriority,
            string threadDescription = null,
            Action<PoolThread> onShiftedToLongRunningOperationsPool = null,
            int longRunningOperationLimit = 0,
            int longRunningOperationCancellationTimeout = 0) : 
                base(processingThreadAmount,
                processingThreadPriority,
                processingMethod,
                onProcessingError,
                dispatchLongRunningThreadTimeout,
                longRunningOperationThreadsPriority,
                threadDescription,
                onShiftedToLongRunningOperationsPool,
                longRunningOperationLimit,
                longRunningOperationCancellationTimeout)
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