using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Infrastructure.Configurations
{
    internal class NotificationConfiguration : EntityConfiguration<Notification>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Notification> builder)
        {
            builder.HasOne(x => x.User)
                   .WithMany(x => x.Notifications)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Message)
                   .IsRequired();

            builder.Property(x => x.IsRead).HasDefaultValue(false);

        }
    }
}
