using System.Collections.Concurrent;
using RadFramework.Libraries.Threading.Interfaces;
using RadFramework.Libraries.Threading.Internals;
using RadFramework.Libraries.Threading.Semaphores;
using RadFramework.Libraries.Web;

namespace RadFramework.Libraries.Threading.ThreadPools.Queued
{
    /// <summary>
    /// A MultiThreadProcessor that processes queue tasks on a pool of threads.
    /// </summary>
    /// <typeparam name="TQueueTask">Type of the queue task.</typeparam>
    public class QueuedThreadPool<TQueueTask> : ThreadPoolBase, IQueuedThreadPoolMixinsConsumer<TQueueTask>
    {
        private readonly Action<TQueueTask, PoolThread> processingWorkloadYieldedError;

        /// <summary>
        /// If true the queue processing will stop.
        /// </summary>
        protected bool isDisposed;

        public QueuedThreadPool(
            Action<TQueueTask, PoolThread> processingWorkloadYieldedError,
            Action<TQueueTask> processWorkloadDelegate,
            int processingThreadAmount,
            ThreadPriority processingThreadPriority,
            Action processingDelegate,
            string threadDescription = null) : 
                base(processingThreadAmount,
                processingThreadPriority,
                processingDelegate,
                threadDescription)
        {
            this.processingWorkloadYieldedError = processingWorkloadYieldedError;
            ProcessWorkloadDelegate = processWorkloadDelegate;
        }

        /// <summary>
        /// The queue that feeds the thread pool.
        /// </summary>
        public ConcurrentQueue<TQueueTask> Queue { get; } = new();

        /// <summary>
        /// The delegate that gets called for each workload from the queue
        /// </summary>
        public new Action<TQueueTask> ProcessWorkloadDelegate { get; }

        /// <summary>
        /// A semaphore that unblocks when work arrives.
        /// </summary>
        public CounterSemaphore ProcessIncomingWorkSemaphore { get; }
        

        
        {
            ProcessWorkloadDelegate = task =>
            {
                try
                {
                    onWorkArrivedDelegate(task);
                }
                catch(Exception e)
                {
                    onProcessingError(); //, PoolThread.GetPoolThread(Thread.CurrentThread), e);
                }
            };
            
            ProcessIncomingWorkSemaphore = new CounterSemaphore(Environment.ProcessorCount * threadAmountPerCore);
            
            this.StartThreads();
        }
        
        public override void ProcessingLoop()
        {
            // While were not disposing and thread is part of the pool
            while (!LoopShutdownReasonsApply())
            {
                if (Queue.TryDequeue(out TQueueTask task))
                {
                    this.InvokeProcessWorkloadActionWithErrorHandling(() => ProcessWorkloadDelegate(task));
                    this.TryKickOffProcessingThreads();
                }
                else
                {
                    ProcessIncomingWorkSemaphore.WaitHere();
                }
            }
        }
        
        /// <summary>
        /// This should not be called by the custom loop.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when called.</exception>
        public override void ProcessWorkloadUnit()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Waits until the queue is empty then disposes the resources.
        /// </summary>
        public override void Dispose()
        {
            isDisposed = true;
            
            // dont loose the queue early
            while (!Queue.IsEmpty)
            {
                Thread.Sleep(250);
            }

            base.Dispose();
            
            ProcessIncomingWorkSemaphore.Dispose();
        }
    }
}
