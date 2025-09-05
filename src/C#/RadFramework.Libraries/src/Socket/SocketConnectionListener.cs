using System.Net;
using System.Net.Sockets;
using RadFramework.Libraries.Socket.Interface;
using RadFramework.Libraries.Threading.ThreadPools;

namespace RadFramework.Libraries.Socket;

public class SocketConnectionListener : IDisposable
{
    private readonly OnSocketConnected onSocketConnected;
    private System.Net.Sockets.Socket listenerSocket;

    //private Thread acceptThread;

    private SimpleThreadPool acceptThreadPool;
    
    private bool disposed = false;
    
    public SocketConnectionListener(
        SocketType socketType,
        ProtocolType protocolType,
        OnSocketConnected onSocketConnected,
        int port)
    {
        this.onSocketConnected = onSocketConnected;
        IPEndPoint endPoint = new(IPAddress.Any, port);
        
        listenerSocket = new System.Net.Sockets.Socket(IPAddress.Any.AddressFamily, socketType, protocolType);
        
        listenerSocket.Bind(endPoint);
        
        listenerSocket.Listen();
        
        /*acceptThread = new Thread(AcceptSockets);
        acceptThread.Priority = ThreadPriority.Highest;
        acceptThread.Start();*/

        acceptThreadPool = new SimpleThreadPool(Environment.ProcessorCount * 2, ThreadPriority.Highest, AcceptSockets);
    }

    private void AcceptSockets()
    {
        while (!disposed)
        {
            try
            {
                onSocketConnected(listenerSocket.Accept());
            }
            catch
            {
            }
        }
    }
    
    public void Dispose()
    {
        disposed = true;
        acceptThreadPool.Dispose();
        acceptThreadPool = null;
        listenerSocket.Dispose();
        listenerSocket = null;
    }
}