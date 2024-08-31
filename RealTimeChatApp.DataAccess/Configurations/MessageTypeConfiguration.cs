using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeChatApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.DataAccess.Configurations
{
    internal class MessageTypeConfiguration : EntityConfiguration<MessageType>
    {
        public override void ConfigureEntity(EntityTypeBuilder<MessageType> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(30);
        }
    }
}
