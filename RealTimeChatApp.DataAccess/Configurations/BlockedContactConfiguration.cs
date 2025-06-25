using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Infrastructure.Configurations
{
    internal class BlockedContactConfiguration : EntityConfiguration<BlockedContact>
    {
        public override void ConfigureEntity(EntityTypeBuilder<BlockedContact> builder)
        {
            builder.HasOne(x => x.User)
                   .WithMany(x => x.BlockedContacts)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.BlockedUser)
                   .WithMany()
                   .HasForeignKey(x => x.BlockedUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
