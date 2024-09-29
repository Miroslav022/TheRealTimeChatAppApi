using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Infrastructure.Configurations
{
    internal class ContactConfiguration : EntityConfiguration<Contact>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Contact> builder)
        {
            builder.HasOne(x => x.User)
                   .WithMany(x => x.Contacts)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ContactUser)
                   .WithMany()
                   .HasForeignKey(x => x.ContactUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
