using System.Net;
using System.Net.Sockets;
using RadFramework.Libraries.Socket.Interface;
using RadFramework.Libraries.Threading.ThreadPools;

namespace RadFramework.Libraries.Socket;

public class SocketConnectionListener : IDisposable
{
    private readonly OnNetSocketConnected onNetSocketConnected;
    private System.Net.Sockets.Socket listenerSocket;

    //private Thread acceptThread;

    private SimpleThreadPool acceptThreadPool;
    
    private bool disposed = false;
    
    public SocketConnectionListener(
        IPEndPoint listenerEndpoint,
        SocketType socketType,
        ProtocolType protocolType,
        OnNetSocketConnected onNetSocketConnected)
    {
        this.onNetSocketConnected = onNetSocketConnected;
        IPEndPoint endPoint = listenerEndpoint;
        
        listenerSocket = new System.Net.Sockets.Socket(listenerEndpoint.AddressFamily, socketType, protocolType);
        
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
                onNetSocketConnected(listenerSocket.Accept());
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
        listenerSocket.Shutdown(SocketShutdown.Both);
        listenerSocket.Dispose();
        listenerSocket = null;
    }
}