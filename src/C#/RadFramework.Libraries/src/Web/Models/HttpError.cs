namespace RadFramework.Libraries.Web.Models;

public class HttpError
{
    public HttpConnection Connection { get; set; }

    public System.Net.Sockets.Socket Socket => Connection.UnderlyingSocket;

    public HttpRequest Request => Connection.Request;

    public HttpResponse Response => Connection.Response;

    public Exception Exception { get; set; }
}