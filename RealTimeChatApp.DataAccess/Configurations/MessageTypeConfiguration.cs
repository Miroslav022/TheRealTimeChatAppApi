using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeChatApp.Domain.Entities;

namespace RealTimeChatApp.Infrastructure.Configurations
{
    internal class MessageTypeConfiguration : EntityConfiguration<MessageType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<MessageType> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(30);
        }
    }
}
