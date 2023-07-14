using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Like>().HasKey(k => new {k.ProjectLikedId, k.UserLikedId});

            builder.Entity<Like>()
                .HasOne(u => u.UserLiked)
                .WithMany(l => l.LikedProjects)
                .HasForeignKey(u => u.UserLikedId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Like>()
                .HasOne(p => p.ProjectLiked)
                .WithMany(u => u.LikedByUsers)
                .HasForeignKey(p => p.ProjectLikedId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}