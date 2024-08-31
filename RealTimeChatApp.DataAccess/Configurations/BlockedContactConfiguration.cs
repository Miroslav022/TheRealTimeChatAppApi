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
    internal class BlockedContactConfiguration : EntityConfiguration<BlockedContact>
    {
        public override void ConfigureEntity(EntityTypeBuilder<BlockedContact> builder)
        {
            builder.HasOne(x=>x.User)
                   .WithMany(x=>x.BlockedContacts)
                   .HasForeignKey(x=>x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x=>x.BlockedUser)
                   .WithMany()
                   .HasForeignKey(x=>x.BlockedUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
