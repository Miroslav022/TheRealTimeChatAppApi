﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealTimeChatApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.DataAccess.Configurations
{
    internal class ConversationConfiguration : EntityConfiguration<Conversation>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Conversation> builder)
        {
            builder.HasOne(x => x.CreatedByUser)
                   .WithMany(x => x.Conversations)
                   .HasForeignKey(x => x.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.GroupName).HasMaxLength(30);
            builder.Property(x=>x.IsGroup).HasDefaultValue(false);
            
        }
    }
}