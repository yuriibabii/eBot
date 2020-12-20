using eBot.Data.Domain;
using eBot.Data.Persistent;

namespace eBot.Mappers
{
    public static class VocabularyElementMapper
    {
        public static VocabularyElementDb Map(this VocabularyElement vocabularyElement)
        {
            return new VocabularyElementDb
            {
                Id = vocabularyElement.Id,
                Definition = vocabularyElement.Definition,
                Example = vocabularyElement.Example,
                Transcription = vocabularyElement.Transcription,
                Word = vocabularyElement.Word
            };
        }
        
        public static VocabularyElement Map(this VocabularyElementDb vocabularyElement)
        {
            return new VocabularyElement(
                vocabularyElement.Id,
                vocabularyElement.Word,
                vocabularyElement.Transcription,
                vocabularyElement.Definition,
                vocabularyElement.Example);
        }
    }
}