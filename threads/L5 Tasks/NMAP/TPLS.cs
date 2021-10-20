using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using log4net;

namespace NMAP
{
    public class TPLScanner : SequentialScanner
    {
        protected ILog Log => LogManager.GetLogger(typeof(TPLScanner));

        public override Task Scan(IPAddress[] ipAddrs, int[] ports)
        {
            return Task.WhenAll(ipAddrs.Select(ipAddr => ProcessIpAddr(ipAddr, ports)));
        }

        private Task ProcessIpAddr(IPAddress ipAddr, int[] ports)
        {
            return PingAddrAsync(ipAddr)
                .ContinueWith(pingTask =>
                {
                    if(pingTask.Result != IPStatus.Success)
                        return;

                    Task.WhenAll(ports.Select(p => CheckAsync(ipAddr, p)))
                        .ContinueWith(_ => { }, TaskContinuationOptions.AttachedToParent);
                });
        }

        private Task<IPStatus> PingAddrAsync(IPAddress ipAddr, int timeout = 3000)
        {
            Log.Info($"Pinging {ipAddr}");
            var ping = new Ping();
            return ping
                .SendPingAsync(ipAddr, timeout)
                .ContinueWith(t =>
                {
                    ping.Dispose();
                    var status = t.Result.Status;
                    Log.Info($"Pinged {ipAddr}: {status}");
                    return status;
                });
        }

        private Task CheckAsync(IPAddress ip, int port)
        {
            var tcpClient = new TcpClient();
            Log.Info($"Checking {ip}:{port}");

            return tcpClient
                .ConnectAsync(ip, port, 3000)
                .ContinueWith(t =>
                {
                    PortStatus portStatus;
                    switch (t.Result.Status)
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

                    Log.Info($"Checked {ip}:{port} - {portStatus}");
                })
                .ContinueWith(_ => ((IDisposable) tcpClient).Dispose());
        }
    }
}