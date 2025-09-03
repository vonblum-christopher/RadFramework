using RadFramework.Libraries.Threading.Internals;

namespace RadFramework.Libraries.Web;

public delegate void OnProcessingError(System.Net.Sockets.Socket socket, PoolThread poolThread, Exception exception);