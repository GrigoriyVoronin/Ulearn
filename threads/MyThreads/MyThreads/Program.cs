using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApplication22
{
    class program
    {
                public static void Main()
        {
            new MainDownloader().StartMainDownloader();
        }
    }
    class MainDownloader
    {       

        public void StartMainDownloader()
        {
            string webPath = "https://vk.com";
            Downloader DL_1 = new Downloader(webPath);
            Downloader DL_2 = new Downloader(webPath);
            var b = DL_1.Start();
            var a = DL_2.Start();

            var c = b.GetAwaiter();
            c.GetResult();
            b.Start();

        }
    }
    
class Downloader
    {   
        private string webPath;
        public List<string> MyList;
        
        public Downloader(string newPath)
        {
            webPath = newPath;
        }
        
        public async Task Start()
        {
            WebClient wClient = new WebClient();
            MyList = new List<string>();
            string page;
            await Task.Factory.StartNew(() =>
            {        
                    page = wClient.DownloadString(webPath);
                    MyList.Add(page);
            });
        }
    }
}