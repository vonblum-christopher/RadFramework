using RadFramework.Libraries.Threading.ThreadPools.Internals;

namespace RadFramework.Libraries.Threading.Interface;

public delegate void OnErrorDelegate<TQueueTask>(TQueueTask task, PoolThread poolThread, Exception exception);