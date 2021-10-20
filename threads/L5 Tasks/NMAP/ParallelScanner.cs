using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace NMAP
{
    internal class ParallelScanner : SequentialScanner
    {
        public override Task Scan(IPAddress[] ipAddrs, int[] ports)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var ipAddr in ipAddrs)
                    Task.Factory.StartNew(() =>
                    {
                        if (PingAddr(ipAddr) == IPStatus.Success)
                            foreach (var port in ports)
                                Task.Factory.StartNew(() => CheckPort(ipAddr, port),
                                    TaskCreationOptions.AttachedToParent);
                    }, TaskCreationOptions.AttachedToParent);
            });
        }
    }
}