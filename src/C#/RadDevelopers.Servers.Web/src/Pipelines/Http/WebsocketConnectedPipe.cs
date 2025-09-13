using RadFramework.Libraries.Pipelines;
using RadFramework.Libraries.Pipelines.Base;
using RadFramework.Libraries.Pipelines.Parameters;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class WebsocketConnectedPipe : ExtensionPipeBase<HttpConnection, HttpConnection>
{
    public override void Process(HttpConnection input, ExtensionPipeContext<HttpConnection> pipeContext)
    {
        if (((input.Request.Headers.ContainsKey("Upgrade") && input.Request.Headers["Upgrade"] == "websocket") 
                                           && (input.Request.Headers.ContainsKey("Connection") && input.Request.Headers["Connection"] == "Upgrade")))
        {
            input.DisposeReaderAndStream();
            
            input.UnderlyingSocket.Close();

            // async (use thread pool)
            // pipeContext.Manager.ForkAndProcess<IPipeline>();
            // pipeContext.Manager.Process<IPipeline>
            //ipeContext.Manager.Get().Process
                
            pipeContext.Return();
        }
    }
}