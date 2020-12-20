using eBot.Data.Persistent;
using Microsoft.EntityFrameworkCore;

namespace eBot.DbContexts
{
    public class StudyContext : DbContext
    {
        public StudyContext(DbContextOptions<StudyContext> options) : base(options) { }

        public DbSet<UserDb> Users { get; set; } = null!;

        public DbSet<VocabularyElementDb> Vocabulary { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
        }
    }
}