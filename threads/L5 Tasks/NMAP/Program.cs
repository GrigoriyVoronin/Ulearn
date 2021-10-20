using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace NMAP
{
    class Program
    {
        static void Main(string[] args)
        {
            var ipAddrs = GenIpAddrs();
            var ports = new[] {21/*, 25, 80, 443, 3389*/};
            var t = new Stopwatch();
            t.Start();
            //var scannerS = new SequentialScanner();
            //scannerS.Scan(ipAddrs, ports).Wait();
            //Console.WriteLine(t.Elapsed.TotalSeconds);
            //t.Restart();
            //var scannerP = new ParallelScanner();
            //scannerP.Scan(ipAddrs, ports).Wait();
            var scannerP = new TPLScanner();
            scannerP.Scan(ipAddrs, ports).Wait();
            Console.WriteLine(t.Elapsed.TotalSeconds);
        }

        private static IPAddress[] GenIpAddrs()
        {
            var konturAddrs = new List<IPAddress>();
            uint focusIpInt = 0x0ACB112E;
            for (int b = 0; b <= byte.MaxValue; b++)
                konturAddrs.Add(new IPAddress((focusIpInt & 0x00FFFFFF) | ((uint) b << 24)));
            return konturAddrs.ToArray();
        }
    }
}
