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
    internal class MessageConversation : EntityConfiguration<Message>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Message> builder)
        {
            builder.HasOne(x => x.Conversation)
                   .WithMany(x=>x.Messages)
                   .HasForeignKey(x=>x.ConversationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Sender)
                   .WithMany(x => x.Messages)
                   .HasForeignKey(x => x.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x=>x.MessageType)
                   .WithMany(x=>x.Messages)
                   .HasForeignKey(x=> x.MessageTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x=>x.IsRead).HasDefaultValue(false);

            builder.HasOne(x => x.RepliedToMessage)
                   .WithMany(x => x.Replies)
                   .HasForeignKey(x => x.RepliedToMessageId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);
        }
    }
}
