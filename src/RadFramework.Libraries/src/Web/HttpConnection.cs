using System.Net.Sockets;

namespace RadFramework.Libraries.Web;

public class HttpConnection : IDisposable
{
    public HttpServerContext ServerContext { get; set; }
    private HttpResponse response;
    
    public HttpRequest Request;
    public HttpResponse Response => response ??= new(this);

    public StreamReader RequestReader;
    public NetworkStream UnderlyingStream;
    public System.Net.Sockets.Socket UnderlyingSocket;

    /// <summary>
    /// When the socket turns a web socket we dont need reader and stream anymore 
    /// </summary>
    public void DisposeReaderAndStream()
    {
        RequestReader?.Dispose();
        UnderlyingStream?.Dispose();
    }
    
    public void Dispose()
    {
        DisposeReaderAndStream();
        UnderlyingSocket?.Dispose();
    }
}