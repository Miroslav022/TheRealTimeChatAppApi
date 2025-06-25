using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Infrastructure.Configurations
{
    internal class UserPresenceConfiguration : EntityConfiguration<UserPresence>
    {
        public override void ConfigureEntity(EntityTypeBuilder<UserPresence> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany(x => x.UserPresence)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.IsOnline).HasDefaultValue(false);

        }
    }
}
