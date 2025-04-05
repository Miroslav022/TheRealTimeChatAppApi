using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Infrastructure.Configurations
{
    internal class UserConfiguration : EntityConfiguration<User>
    {
        public override void ConfigureEntity(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Email)
                   .HasMaxLength(40)
                   .IsRequired();

            builder.HasIndex(x => x.Email)
                   .IsUnique();

            builder.Property(x => x.Username)
                   .HasMaxLength(30)
                   .IsRequired();

            builder.Property(x => x.PasswordHash)
                   .HasMaxLength(120)
                   .IsRequired();

            builder.Property(x => x.PasswordSalt)
                   .HasMaxLength(30)
                   .IsRequired();

            builder.Property(x => x.IsBlocked)
                   .HasDefaultValue(false);

            builder.Property(x => x.ProfilePicture)
                   .HasDefaultValue("defaultAvatar.png")
                   .IsRequired();

            builder.Property(x => x.IsActive).HasDefaultValue(true);

        }
    }
}
