using System.Net.Sockets;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.WebSocketConnected;

public class WebSocketConnects
{
    public Socket Socket { get; set; }
    public HttpRequest Request { get; set; }
}