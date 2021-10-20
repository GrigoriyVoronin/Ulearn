using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using log4net;

namespace NMAP
{
    public class AsyncScanner : IPScanner
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AsyncScanner));

        public async Task Scan(IPAddress[] ipAddrs, int[] ports)
        {
            await Task.WhenAll(ipAddrs.Select(ipAddr => ProcessIpAddrAsync(ipAddr, ports)));
        }

        private static async Task ProcessIpAddrAsync(IPAddress ipAddr, int[] ports)
        {
            var status = await PingAddrAsync(ipAddr);
            if (status != IPStatus.Success)
                return;

            await Task.WhenAll(ports.Select(port => CheckPortAsync(ipAddr, port)));
        }

        private static async Task<PortStatus> CheckPortAsync(IPAddress ipAddr, int port, int timeout = 3000)
        {
            using (var tcpClient = new TcpClient())
            {
                log.Info($"Checking {ipAddr}:{port}");

                var connectTask = await TcpClientExtensions.ConnectAsync(tcpClient, ipAddr, port);
                PortStatus portStatus;
                switch (connectTask.Status)
                {
                    case TaskStatus.RanToCompletion:
                        portStatus = PortStatus.OPEN;
                        break;
                    case TaskStatus.Faulted:
                        portStatus = PortStatus.CLOSED;
                        break;
                    default:
                        portStatus = PortStatus.FILTERED;
                        break;
                }
                log.Info($"Checked {ipAddr}:{port} - {portStatus}");
                return portStatus;
            }
        }
        
        static async Task<IPStatus> PingAddrAsync(IPAddress ipAddr, int timeout = 3000)
        {
            log.Info($"Pinging {ipAddr}");
            using (var ping = new Ping())
            {
                var status = (await ping.SendPingAsync(ipAddr, timeout)).Status;
                log.Info($"Pinged {ipAddr}: {status}");
                return status;
            }
        }
    }
}