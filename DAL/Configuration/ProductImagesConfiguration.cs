using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class ProductImagesConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.HasKey(pi => pi.Id);
            builder.Property(pi => pi.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(pi => pi.ImageUrl)
                .IsRequired();

            builder.HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}