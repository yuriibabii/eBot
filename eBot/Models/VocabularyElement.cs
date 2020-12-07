using System;

namespace eBot.Models
{
    public class VocabularyElement
    {
        public int Id { get; set; }
        
        public string Word { get; set; } = null!;

        public string Transcription { get; set; } = null!;

        public string Definition { get; set; } = null!;

        public string Example { get; set; } = null!;

        public override string ToString()
        {
            return $"{Word} - [{Transcription}]{Environment.NewLine}Definition: {Definition}{Environment.NewLine}Example: {Example}";
        }
    }
}