using System.Net;
using System.Threading.Tasks;

namespace NMAP
{
    public interface IPScanner
    {
        Task Scan(IPAddress[] ipAdrrs, int[] ports);
    }
}