using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NMAP
{
    public class SequentialScanner : IPScanner
    {
        public virtual Task Scan(IPAddress[] ipAddrs, int[] ports)
        {
            return Task.Run(() =>
            {
                foreach (var ipAddr in ipAddrs)
                {
                    if (PingAddr(ipAddr) != IPStatus.Success)
                        continue;

                    foreach (var port in ports)
                        CheckPort(ipAddr, port);
                }
            });
        }

        protected IPStatus PingAddr(IPAddress ipAddr, int timeout = 3000)
        {
            Console.WriteLine($"Pinging {ipAddr}");
            using (var ping = new Ping())
            {
                var status = ping.Send(ipAddr, timeout).Status;
                Console.WriteLine($"Pinged {ipAddr}: {status}");
                return status;
            }
        }

        protected void CheckPort(IPAddress ipAddr, int port, int timeout = 3000)
        {
            using (var tcpClient = new TcpClient())
            {
                Console.WriteLine($"Checking {ipAddr}:{port}");
                var connectTask = tcpClient.Connect(ipAddr, port, timeout);
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