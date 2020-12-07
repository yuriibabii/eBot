using System;
using eBot.Extensions;

namespace eBot.Models
{
    public class RememberElement
    {
        public RememberElement(VocabularyElement vocabularyElement)
        {
            VocabularyElement = vocabularyElement;
        }
        
        public VocabularyElement VocabularyElement { get; }

        public int Id => VocabularyElement.Id;
        
        public RememberProgress Progress { get; set; }
        
        public DateTimeOffset LastTimeRepeated { get; set; }

        public string DefinitionWithoutWord => 
            VocabularyElement.Definition.ReplaceWordWithUnderscore(VocabularyElement.Word);

        public string ExampleWithoutWord => 
            VocabularyElement.Example.ReplaceWordWithUnderscore(VocabularyElement.Word);

        public override string ToString()
        {
            return $"{DefinitionWithoutWord}{Environment.NewLine}{ExampleWithoutWord}";
        }
    }
}