using CareNest_Review.Domain.Entitites;
using Microsoft.EntityFrameworkCore;


namespace CareNest_Review.Infrastructure.Persistences.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Review> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Config Review entity
            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Appointments");
                
                // Đảm bảo tên cột giữ nguyên PascalCase như trong migration
                entity.Property(e => e.ItemDetailId)
                    .HasColumnName("ItemDetailId");
                entity.Property(e => e.CustomerId)
                    .HasColumnName("CustomerId");
                entity.Property(e => e.Rating)
                    .HasColumnName("Rating");
                entity.Property(e => e.Contents)
                    .HasColumnName("Contents");
                entity.Property(e => e.ImgUrl)
                    .HasColumnName("ImgUrl");
                entity.Property(e => e.Type)
                    .HasColumnName("Type");
            });
        }
    }
}
