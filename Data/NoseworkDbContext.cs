using Data.Ent;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class NoseworkDbContext : DbContext
    {
        public NoseworkDbContext(DbContextOptions<NoseworkDbContext> options) : base(options) { }

        public DbSet<SessionEnt> Sessions { get; set; }
        public DbSet<DifficultyLevelEnt> DifficultyLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SessionEnt>()
                .HasMany(s => s.Difficulties)
                .WithMany(d => d.Sessions);

            modelBuilder.Entity<DifficultyLevelEnt>().HasData(
                new DifficultyLevelEnt { Id = 1, CategoryName = "Beginner" },
                new DifficultyLevelEnt { Id = 2, CategoryName = "Medium" },
                new DifficultyLevelEnt { Id = 3, CategoryName = "Advanced" }
            );
        }
    }
}
