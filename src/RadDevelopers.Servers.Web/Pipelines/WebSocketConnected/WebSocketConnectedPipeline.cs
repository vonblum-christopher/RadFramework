using RadFramework.Libraries.Patterns.Pipeline;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.WebSocketConnected;

public class WebsocketConnectedPipe: IHttpPipe
{
    public void Process(HttpConnection connection, ExtensionPipeContext pipeContext)
    {
        connection.Dispose();
    }
    
    
}