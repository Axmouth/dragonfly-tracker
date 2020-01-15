using DragonflyTracker.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DragonflyTracker.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public DbSet<IssuePost> IssuePosts { get; set; }

        public DbSet<IssueUpdate> IssueUpdates { get; set; }

        public DbSet<IssuePostReactions> IssuePostReactions { get; set; }

        public DbSet<IssueStage> IssueStages { get; set; }

        public DbSet<IssueType> IssueTypes { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PostTag>().Ignore(xx => xx.Post).HasKey(x => new { x.PostId, x.TagName });
        }
    }
}