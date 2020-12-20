namespace eBot.Data.Persistent
{
    public class VocabularyElementDb
    {
        public long Id { get; set; }
        
        public string Word { get; set; } = null!;

        public string Transcription { get; set; } = null!;

        public string Definition { get; set; } = null!;

        public string Example { get; set; } = null!;
    }
}