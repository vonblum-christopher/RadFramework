using RadFramework.Libraries.Extensibility.Pipeline.Extension;

namespace RadFramework.Libraries.Web;

public interface IHttpErrorPipe : IExtensionPipe<(HttpConnection connection, Exception e)>
{
}