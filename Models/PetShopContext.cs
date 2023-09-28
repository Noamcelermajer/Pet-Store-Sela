using Microsoft.EntityFrameworkCore;

namespace pet_store_noamcelermajer.Models
{
    public class PetShopContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public PetShopContext(DbContextOptions<PetShopContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Database/PetShopDB.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).HasColumnType("TEXT");
            });
        }
    }
}
