using RadFramework.Libraries.Patterns.Pipeline;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.WebSocketConnected;

public class WebsocketConnectedPipe: IHttpPipe
{
    public void Process(HttpConnection input, ExtensionPipeContext pipeContext)
    {
        input.Dispose();
    }
    
    
}