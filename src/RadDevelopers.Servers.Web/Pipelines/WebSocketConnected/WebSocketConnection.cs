using System.Net.Sockets;
using RadFramework.Libraries.Web;

namespace RadDevelopers.Servers.Web.Pipelines.Http;

public class WebSocketConnection
{
    public Socket Socket { get; set; }
    public HttpRequest Request { get; set; }
}