using System;
using eBot.Data.Enums;

namespace eBot.Data.Domain
{
    public class CustomStudyElement : IStudyElement
    {
        public CustomStudyElement(string word)
        {
            Word = word;
        }

        public RememberProgress Progress { get; set; }

        public DateTimeOffset LastTimeRepeated { get; set; }

        public string Word { get; }
    }
}
