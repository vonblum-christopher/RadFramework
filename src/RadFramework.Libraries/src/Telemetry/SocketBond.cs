namespace RadFramework.Libraries.Telemetry;

public class SocketBond
{
    public System.Net.Sockets.Socket SendSocket { get; set; }
    public System.Net.Sockets.Socket ReceiveSocket { get; set; }

    public void Dispose()
    {
        SendSocket.Close();
        ReceiveSocket.Close();
        SendSocket.Dispose();
        ReceiveSocket.Dispose();
    }
}