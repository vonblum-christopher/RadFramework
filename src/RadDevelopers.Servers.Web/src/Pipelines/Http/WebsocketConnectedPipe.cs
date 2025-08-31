using RadFramework.Libraries.Patterns.Pipeline;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class WebsocketConnectedPipe: IHttpPipe
{
    public WebsocketConnectedPipe()
    {
        
    }
    
    public void Process(HttpConnection input, ExtensionPipeContext pipeContext)
    {
        if (((input.Request.Headers.ContainsKey("Upgrade") && input.Request.Headers["Upgrade"] == "websocket") 
                                           && (input.Request.Headers.ContainsKey("Connection") && input.Request.Headers["Connection"] == "Upgrade")))
        {
            input.DisposeReaderAndStream();
            
            input.UnderlyingSocket
            
            pipeContext.Return();
        }
    }
}