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
                
                // PostgreSQL tự động convert tên cột thành lowercase nếu không có quotes
                // Map về tên cột lowercase để khớp với database thực tế
                entity.Property(e => e.Id)
                    .HasColumnName("id");
                entity.Property(e => e.ItemDetailId)
                    .HasColumnName("itemdetailid");
                entity.Property(e => e.CustomerId)
                    .HasColumnName("customerid");
                entity.Property(e => e.Rating)
                    .HasColumnName("rating");
                entity.Property(e => e.Contents)
                    .HasColumnName("contents");
                entity.Property(e => e.ImgUrl)
                    .HasColumnName("imgurl");
                entity.Property(e => e.Type)
                    .HasColumnName("type");
                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdby");
                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updatedby");
                entity.Property(e => e.DeletedBy)
                    .HasColumnName("deletedby");
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("createdat");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updatedat");
                entity.Property(e => e.DeleteAt)
                    .HasColumnName("deleteat");
            });
        }
    }
}
