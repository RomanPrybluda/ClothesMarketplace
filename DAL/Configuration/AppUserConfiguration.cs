using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder
                .HasKey(u => u.Id);

            builder
                .Property(u => u.UserName)
                .HasMaxLength(100);

            builder
                .HasIndex(u => u.UserName)
                .IsUnique();

            builder
                .Property(u => u.Email)
                .HasMaxLength(255);

            builder
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
