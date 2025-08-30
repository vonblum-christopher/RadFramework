namespace RadFramework.Libraries.Web;

public class HttpError
{
    public HttpConnection Connection { get; internal set; }

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