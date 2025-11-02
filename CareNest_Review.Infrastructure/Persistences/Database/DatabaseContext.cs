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
                
                // PostgreSQL thường dùng snake_case cho tên cột
                // Map các cột về snake_case để khớp với database thực tế
                entity.Property(e => e.ItemDetailId)
                    .HasColumnName("item_detail_id");
                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id");
                entity.Property(e => e.Rating)
                    .HasColumnName("rating");
                entity.Property(e => e.Contents)
                    .HasColumnName("contents");
                entity.Property(e => e.ImgUrl)
                    .HasColumnName("img_url");
                entity.Property(e => e.Type)
                    .HasColumnName("type");
                entity.Property(e => e.CreatedBy)
                    .HasColumnName("created_by");
                entity.Property(e => e.UpdatedBy)
                    .HasColumnName("updated_by");
                entity.Property(e => e.DeletedBy)
                    .HasColumnName("deleted_by");
                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at");
                entity.Property(e => e.DeleteAt)
                    .HasColumnName("delete_at");
            });
        }
    }
}
