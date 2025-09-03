using RadFramework.Libraries.Threading.Internals;

namespace RadFramework.Libraries.Web;

public delegate OnProcessingError<TQueueTask>(TQueueTask task, PoolThread poolThread, Exception exception);