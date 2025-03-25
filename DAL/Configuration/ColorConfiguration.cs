using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(c => c.Products)
                .WithOne(p => p.Color)
                .HasForeignKey(p => p.ColorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
