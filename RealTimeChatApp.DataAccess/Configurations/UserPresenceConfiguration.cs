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
    internal class UserPresenceConfiguration : EntityConfiguration<UserPresence>
    {
        public override void ConfigureEntity(EntityTypeBuilder<UserPresence> builder)
        {
            builder.HasOne(x=>x.User)
                .WithMany(x=>x.UserPresence)
                .HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x=>x.IsOnline).HasDefaultValue(false);

        }
    }
}
