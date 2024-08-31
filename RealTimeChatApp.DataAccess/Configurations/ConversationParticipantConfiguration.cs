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
    internal class ConversationParticipantConfiguration : EntityConfiguration<ConversationParticipant>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ConversationParticipant> builder)
        {
            builder.HasOne(x => x.Conversation)
                   .WithMany(x => x.Participants)
                   .HasForeignKey(x => x.ConversationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x=>x.User)
                   .WithMany(x=>x.ConversationParticipants)
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x=>x.IsAdmin).HasDefaultValue(false);

        }
    }
}
