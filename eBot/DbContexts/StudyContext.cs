using eBot.Models;
using Microsoft.EntityFrameworkCore;

namespace eBot.DbContexts
{
    public class StudyContext : DbContext
    {
        public StudyContext(DbContextOptions<StudyContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<VocabularyElement> Vocabulary { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
        }
    }
}