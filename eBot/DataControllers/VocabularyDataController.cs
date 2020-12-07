using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CsvHelper;
using eBot.DbContexts;
using eBot.Extensions;
using eBot.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace eBot.DataControllers
{
    public class VocabularyDataController : IVocabularyDataController
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<VocabularyDataController> logger;
        
        public VocabularyDataController(IServiceScopeFactory serviceScopeFactory, ILogger<VocabularyDataController> logger)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }
        
        public async Task LoadEssentialVocabularySetAsync()
        {
            using var serviceScope = serviceScopeFactory.CreateScope();
            var studyContext = serviceScope.ServiceProvider.Resolve<StudyContext>();
            if (studyContext.Vocabulary.Any())
            {
                return;
            }
            
            await using var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream(AppSettings.EssentialVocabularyName);

            if (stream == null)
            {
                return;
            }
            
            using var streamReader = new StreamReader(stream);
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            try
            {
                var vocabularyElements = csvReader.GetRecords<VocabularyElement>().ToList();
                
                await studyContext.Vocabulary.AddRangeAsync(vocabularyElements);
                await studyContext.SaveChangesAsync();
            }
            catch (BadDataException exception)
            {
                logger.LogError(exception.Message);
            }
        }
    }
}