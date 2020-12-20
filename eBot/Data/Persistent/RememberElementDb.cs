using System;
using eBot.Data.Enums;

namespace eBot.Data.Persistent
{
    public class RememberElementDb
    {
        public long Id { get; set; }

        public long VocabularyElementId { get; set; }
        
        public RememberProgress Progress { get; set; }
        
        public DateTimeOffset LastTimeRepeated { get; set; }
    }
}