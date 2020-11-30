using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using eBot.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace eBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = BuildWebHost(args);
            await LoadEssentialVocabularySetAsync();
            webHost.Run();
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

        private static Task LoadEssentialVocabularySetAsync()
        {
            var loadTask = Task.Run(() =>
            {
                var stream = Assembly
                    .GetExecutingAssembly()
                    .GetManifestResourceStream(AppSettings.EssentialVocabularyName);
                
                var streamReader = new StreamReader(stream);
                var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    BadDataFound = OnBadDataFound,
                };

                var csvReader = new CsvReader(streamReader, csvConfiguration);
                try
                {
                    var vocabularyElements = csvReader.GetRecords<VocabularyElement>().ToList();
                }
                catch (BadDataException exception)
                {
                    
                }
            });

            return loadTask;
        }

        private static void OnBadDataFound(ReadingContext obj)
        {
            
        }
    }
}
