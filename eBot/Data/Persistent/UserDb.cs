using System.Collections.Generic;
using eBot.Commands;
using eBot.Data.Domain;

namespace eBot.Data.Persistent
{
    public class UserDb
    {
        public long Id { get; set; }

        public IList<RememberElementDb> ElementsInProgress { get; set; }

        public string CompletelyRememberedElements { get; set; } = null!;

        public string LastCommand { get; set; } = null!;

        public string LastCommandTypeName { get; set; } = null!;
    }
}