using System.Net.Sockets;
using RadFramework.Libraries.Pipelines;
using RadFramework.Libraries.Pipelines.Base;
using RadFramework.Libraries.Pipelines.Parameters;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.WebSocketConnected;

public class WebsocketConnectedPipe : ExtensionPipeBase<(HttpConnection connection, Socket socket), (HttpConnection connection, Socket socket)>
{
    public override void Process((HttpConnection connection, Socket socket) input, ExtensionPipeContext<(HttpConnection connection, Socket socket)> pipeContext)
    {
        input.socket.Close();
    }
}