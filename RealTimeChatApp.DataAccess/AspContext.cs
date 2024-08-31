using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RealTimeChatApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatApp.DataAccess
{
    public class AspContext : DbContext
    {

        public AspContext(DbContextOptions<AspContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }
        public override int SaveChanges()
        {
            IEnumerable<EntityEntry> entries = this.ChangeTracker.Entries();
            foreach (EntityEntry entry in entries) { 
                if(entry.State == EntityState.Added)
                {
                    if(entry.Entity is Entity e)
                    {
                        e.IsDeleted = false;
                        e.CreatedAt = DateTime.UtcNow;
                    }

                }
                if(entry.State == EntityState.Modified)
                {
                    if(entry.Entity is Entity e)
                    {
                        e.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }
            return base.SaveChanges();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<BlockedContact> BlockedContacts { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationParticipant> ConversationParticipants { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserPresence> UserPresences { get; set; }
        public DbSet<MessageType> MessageTypes { get; set; }
        public DbSet<Media> Medias { get; set; }




    }
}
