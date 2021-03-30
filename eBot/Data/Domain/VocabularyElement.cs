using System;

namespace eBot.Data.Domain
{
    public class VocabularyElement
    {
        public VocabularyElement(long id, string word, string transcription, string definition, string example)
        {
            Id = id;
            Word = word;
            Transcription = transcription;
            Definition = definition;
            Example = example;
        }

        public long Id { get; }

        public string Word { get; }

        public string Transcription { get; }

        public string Definition { get; }

        public string Example { get; }

        public override string ToString() =>
            $"{Word} - [{Transcription}]{Environment.NewLine}Definition: {Definition}{Environment.NewLine}Example: {Example}";
    }
}