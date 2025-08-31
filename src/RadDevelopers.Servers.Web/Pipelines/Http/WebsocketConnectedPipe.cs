using RadFramework.Libraries.Extensibility.Pipeline.Extension;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class WebsocketConnectedPipe: IHttpPipe
{
    public void Process(HttpConnection connection, ExtensionPipeContext pipeContext)
    {
        if (((connection.Request.Headers.ContainsKey("Upgrade") && connection.Request.Headers["Upgrade"] == "websocket") 
                                           && (connection.Request.Headers.ContainsKey("Connection") && connection.Request.Headers["Connection"] == "Upgrade")))
        {
            connection.DisposeReaderAndStream();
            
            connection.UnderlyingSocket
            
            pipeContext.Return();
        }
    }
}