using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NMAP
{
    static class TcpClientExtensions
    {
        public static Task Connect(this TcpClient tcpClient, IPAddress ipAddr, int port, int timeout = 3000)
        {
            var connectTask = tcpClient.ConnectAsync(ipAddr, port);
            Task.WaitAny(connectTask, Task.Delay(timeout));
            return connectTask;
        }

        public static async Task<Task> ConnectAsync(this TcpClient tcpClient, IPAddress ipAddr, int port, int timeout = 3000)
        {
            var connectTask = tcpClient.ConnectAsync(ipAddr, port);
            await Task.WhenAny(connectTask, Task.Delay(timeout));
            return connectTask;
        }
    }
}