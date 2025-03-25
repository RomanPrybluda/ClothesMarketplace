using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class ForWhomConfiguration : IEntityTypeConfiguration<ForWhom>
    {
        public void Configure(EntityTypeBuilder<ForWhom> builder)
        {
            builder.HasKey(fw => fw.Id);

            builder.Property(fw => fw.Id)
                .HasDefaultValueSql("NEWID()")
                .ValueGeneratedOnAdd();

            builder.Property(fw => fw.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(fw => fw.Products)
                .WithOne(p => p.ForWhom)
                .HasForeignKey(p => p.ForWhomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
