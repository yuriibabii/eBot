using System;
using eBot.Data.Enums;

namespace eBot.Data.Domain
{
    public interface IStudyElement
    {
        public RememberProgress Progress { get; set; }

        public DateTimeOffset LastTimeRepeated { get; set; }

        public string Word { get; }
    }
}
