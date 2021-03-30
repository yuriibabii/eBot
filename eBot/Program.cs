using System.IO;
using System.Threading.Tasks;
using eBot.Extensions;
using eBot.Repositories;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace eBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = BuildWebHost(args);
            var vocabularyDataController = webHost.Services.Resolve<IVocabularyRepository>();
            await vocabularyDataController.LoadEssentialVocabularySetAsync();
            await webHost.RunAsync();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
    }
}
