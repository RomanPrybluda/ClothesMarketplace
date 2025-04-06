using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class ProductConditionConfiguration : IEntityTypeConfiguration<ProductCondition>
    {
        public void Configure(EntityTypeBuilder<ProductCondition> builder)
        {
            builder.HasKey(pc => pc.Id);

            builder.Property(pc => pc.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(pc => pc.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(pc => pc.Products)
                .WithOne(p => p.ProductCondition)
                .HasForeignKey(p => p.ProductConditionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
