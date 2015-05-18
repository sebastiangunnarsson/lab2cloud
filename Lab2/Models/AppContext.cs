using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab2.Models
{
    public class AppContext : IdentityDbContext<CustomIdentityUser>
    {

        public AppContext()
            : base("AppContext")
        {
            
        }
        public DbSet<User> AppUsers { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<Memory> Memories { get; set; }
        public DbSet<SearchTag> SearchTags { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Shard> Shards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasMany(n => n.Friends)
                .WithMany()
                .Map(m => m.MapLeftKey("UserId").MapRightKey("FriendId").ToTable("UserFriends"));
        }
    }
}