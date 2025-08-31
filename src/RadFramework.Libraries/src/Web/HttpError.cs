namespace RadFramework.Libraries.Web;

public class HttpError
{
    public HttpConnection Connection { get; internal set; }

    public System.Net.Sockets.Socket Socket
    {
        get
        {
            return Connection.UnderlyingSocket;
        }
    }

    public HttpRequest Request
    {
        get
        {
            return Connection.Request;
        }
    }
    
    public HttpResponse Response
    {
        get
        {
            return Connection.Response;
        }
    }

    public Exception Exception { get; internal set; }
}