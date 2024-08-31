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
    internal class MediaConfiguration : EntityConfiguration<Media>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Media> builder)
        {
            builder.HasOne(x=>x.Message)
                   .WithMany(x=>x.Media)
                   .HasForeignKey(x=>x.MessageId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Url).IsRequired();

            builder.HasOne(x => x.MediaType)
                   .WithMany(x=>x.Medias)
                   .HasForeignKey(x => x.MediaTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
