using RadFramework.Libraries.Extensibility.Pipeline.Extension;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class WebsocketConnectedPipe: IHttpPipe
{
    public void Process(HttpConnection connection, ExtensionPipeContext pipeContext)
    {
        connection.Dispose();
    }
    
    
}