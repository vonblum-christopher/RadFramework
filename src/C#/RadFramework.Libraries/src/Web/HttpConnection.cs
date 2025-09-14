using System.Net.Sockets;

namespace RadFramework.Libraries.Web;

public class HttpConnection : IDisposable
{
    public HttpServerContext ServerContext { get; set; }
    private HttpResponse response;
    
    public HttpRequest Request { get; set; }
    public HttpResponse Response => response ??= new HttpResponse(this);

    public StreamReader RequestReader;
    public NetworkStream UnderlyingStream;
    public System.Net.Sockets.Socket UnderlyingSocket;

    /// <summary>
    /// When the socket turns a web socket we dont need reader and stream anymore 
    /// </summary>
    public void DisposeReaderAndStream()
    {
        RequestReader?.Dispose();
        RequestReader = null;
        UnderlyingStream?.Dispose();
        UnderlyingStream = null;
    }
    
    public void Dispose()
    {
        DisposeReaderAndStream();
        UnderlyingSocket?.Dispose();
        UnderlyingSocket = null;
    }
}