using RadFramework.Libraries.Abstractions;

namespace RadFramework.Libraries.Threading.ThreadPools
{
    /// <summary>
    /// Dummy class to execute everything on the current thread.
    /// </summary>
    public class StayInCurrentThreadDelegateSheduler : IDelegateSheduler
    {
        /// <summary>
        /// Executes the task delegate.
        /// </summary>
        /// <param name="task">The task delegate</param>
        public void Enqueue(Action task)
        {
            task();
        }

        /// <summary>
        /// Executes the task delegates.
        /// </summary>
        /// <param name="tasks">The task delegates</param>
        public void Enqueue(IEnumerable<Action> tasks)
        {
            foreach (var task in tasks)
            {
                Enqueue(task);
            }
        }

        public void Dispose()
        {
        }
    }
}