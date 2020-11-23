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
                .UseUrls("http://5d1815ce8ea5.ngrok.io")
//                .UseKestrel()
//                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}
