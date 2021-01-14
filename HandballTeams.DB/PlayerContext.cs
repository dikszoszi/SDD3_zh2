using Microsoft.EntityFrameworkCore;

[assembly: System.CLSCompliant(false)]
namespace HandballTeams.DB
{
    public partial class PlayerContext : DbContext
    {
        public virtual DbSet<Player> Players { get; set; }

        public PlayerContext()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder is null) throw new System.ArgumentNullException(nameof(optionsBuilder));
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies()
                    .UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename=|DataDirectory|\PlayerDb.mdf; Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null) throw new System.ArgumentNullException(nameof(modelBuilder));
            Player player = new Player
            {
                Id = 1,
                FamilyName = "Akaratos",
                FirstName = "Aranka",
                Position = "Centre",
                Salary = 9999
            };
            modelBuilder.Entity<Player>().HasData(player);
        }
    }
}
