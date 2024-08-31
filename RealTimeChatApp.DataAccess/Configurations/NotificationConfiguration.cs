using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeChatApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.DataAccess.Configurations
{
    internal class NotificationConfiguration : EntityConfiguration<Notification>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Notification> builder)
        {
            builder.HasOne(x=>x.User)
                   .WithMany(x=>x.Notifications)
                   .HasForeignKey(x=>x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Message)
                   .IsRequired();

            builder.Property(x=>x.IsRead).HasDefaultValue(false);

        }
    }
}
