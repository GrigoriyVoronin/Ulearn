using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NMAP
{
    internal class ParallelScanner : SequentialScanner
    {
        public override async Task Scan(IPAddress[] ipAddrs, int[] ports)
        {
            await Task.WhenAll(ipAddrs
                .Select(ipAddr => AsyncPingAddr(ipAddr)
                    .ContinueWith((task) =>
                    {
                        if (task.Result == IPStatus.Success)
                            Task.WhenAll(ports
                                .Select(port => AsyncCheckPort(ipAddr, port)));
                    })));
        }

        protected async Task<IPStatus> AsyncPingAddr(IPAddress ipAddr, int timeout = 3000)
        {
            Console.WriteLine($"Pinging {ipAddr}");
            using (var ping = new Ping())
            {
                var status = (await ping.SendPingAsync(ipAddr, timeout)).Status;
                Console.WriteLine($"Pinged {ipAddr}: {status}");
                return status;
            }
        }

        private async Task AsyncCheckPort(IPAddress ipAddr, int port, int timeout = 3000)
        {
            using (var tcpClient = new TcpClient())
            {
                Console.WriteLine($"Checking {ipAddr}:{port}");
                var connectTask = await tcpClient.ConnectAsync(ipAddr, port, timeout);
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

                Console.WriteLine($"Checked {ipAddr}:{port} - {portStatus}");
            }
        }
    }
}