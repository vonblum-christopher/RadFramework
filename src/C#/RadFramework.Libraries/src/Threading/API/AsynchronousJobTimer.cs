namespace RadFramework.Libraries.Threading.Timers
{
    public class AsynchronousJobTimer : JobTimer
    {
        public AsynchronousJobTimer(
            int interval,
            Action onIntervalPassed,
            ThreadPriority handlerThreadPriority,
            Action<Thread, Exception> onHandlerThreadException = null) : base(interval,
            onIntervalPassed,
            handlerThreadPriority,
            onHandlerThreadException)
        {
        }

        protected override void OnIntervalPassedInternal()
        {
            Thread handlerThread = new(o => base.OnIntervalPassedInternal());
            handlerThread.Priority = HandlerThreadPriority;
            handlerThread.Start();
        }
    }
}