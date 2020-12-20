using eBot.Data.Domain;
using eBot.Data.Persistent;

namespace eBot.Mappers
{
    public static class RememberElementMapper
    {
        public static RememberElementDb Map(this RememberElement rememberElement)
        {
            return new RememberElementDb
            {
                Id = rememberElement.Id,
                LastTimeRepeated = rememberElement.LastTimeRepeated,
                Progress = rememberElement.Progress,
                VocabularyElementId = rememberElement.VocabularyElement.Id
            };
        }
        
        public static RememberElement Map(this RememberElementDb rememberElement, VocabularyElementDb vocabularyElementDb)
        {
            return new RememberElement(vocabularyElementDb.Map())
            {
                LastTimeRepeated = rememberElement.LastTimeRepeated,
                Progress = rememberElement.Progress
            };
        }
    }
}