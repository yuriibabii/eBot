using eBot.Data.Domain;
using eBot.Data.Persistent;

namespace eBot.Mappers
{
    public static class RememberElementMapper
    {
        public static RememberElementDb Map(this VocabStudyElement rememberElement)
        {
            return new RememberElementDb
            {
                Id = rememberElement.Id,
                LastTimeRepeated = rememberElement.LastTimeRepeated,
                Progress = rememberElement.Progress,
                VocabularyElementId = rememberElement.VocabularyElement.Id
            };
        }
        
        public static VocabStudyElement Map(this RememberElementDb rememberElement, VocabularyElementDb vocabularyElementDb)
        {
            return new VocabStudyElement(vocabularyElementDb.Map())
            {
                LastTimeRepeated = rememberElement.LastTimeRepeated,
                Progress = rememberElement.Progress
            };
        }
    }
}