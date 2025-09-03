using RadFramework.Libraries.Threading.Internals;

namespace RadFramework.Libraries.Web;

public delegate (TQueueTask task, PoolThread poolThread, Exception exception) OnProcessingError<TQueueTask>();