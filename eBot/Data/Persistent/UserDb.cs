using System.Collections.Generic;
using eBot.Commands;
using eBot.Data.Domain;

namespace eBot.Data.Persistent
{
    public class UserDb
    {
        public long Id { get; set; }

        public IList<RememberElementDb> ElementsInProgress { get; set; } = null!;

        public string CompletelyRememberedElements { get; set; } = null!;

        public IBotCommand LastCommand { get; set; } = null!;
    }
}