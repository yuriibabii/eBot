using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CsvHelper;
using eBot.Data.Domain;
using eBot.Data.Persistent;
using eBot.DbContexts;
using eBot.Extensions;
using eBot.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace eBot.DataControllers
{
    public class VocabularyRepository : IVocabularyRepository
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<VocabularyRepository> logger;
        
        public VocabularyRepository(IServiceScopeFactory serviceScopeFactory, ILogger<VocabularyRepository> logger)
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
                var vocabularyElementsDb = csvReader
                    .GetRecords<VocabularyElementDb>();
                
                await studyContext.Vocabulary.AddRangeAsync(vocabularyElementsDb);
                await studyContext.SaveChangesAsync();
            }
            catch (BadDataException exception)
            {
                logger.LogError(exception.Message);
            }
        }
    }
}