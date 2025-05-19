using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class ProductDetailsConfiguration : IEntityTypeConfiguration<ProductDetails>
    {
        public void Configure(EntityTypeBuilder<ProductDetails> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(d => d.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(d => d.Description)
                .HasMaxLength(1000);

            builder.Property(d => d.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(d => d.UpdatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnUpdate();

            builder.Property(d => d.IsActive)
                .HasDefaultValue(true);

            builder.HasOne(d => d.Product)
                .WithOne(p => p.ProductDetails)
                .HasForeignKey<Product>(p => p.ProductDetailsId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
