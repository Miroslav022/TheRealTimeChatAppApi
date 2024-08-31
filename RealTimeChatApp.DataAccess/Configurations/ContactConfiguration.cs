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
