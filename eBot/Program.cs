using System.IO;
using eBot.Models;
using IDM.SkPublish.API;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace eBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

        private static void DownloadEnglishDictionary()
        {
            var api = new SkPublishAPI(AppSettings.CambridgeEnglishDictionaryBaseUrl, );
        }
    }
}
