using DragonflyTracker.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace DragonflyTracker.Data
{
    public class PgMainDataContext : IdentityDbContext
    {
        public PgMainDataContext(DbContextOptions<PgMainDataContext> options) :base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public DbSet<IssuePost> IssuePosts { get; set; }

        public DbSet<IssueUpdate> IssueUpdates { get; set; }

        public DbSet<IssuePostReaction> IssuePostReactions { get; set; }

        public DbSet<IssueStage> IssueStages { get; set; }

        public DbSet<IssueType> IssueTypes { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<ProjectAdmin> ProjectAdmins { get; set; }

        public DbSet<ProjectMaintainer> ProjectMaintainers { get; set; }

        public DbSet<IssueIssueType> IssueIssueTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseNpgsql("User ID=Axmouth;Password=axmouth;Host=localhost;Port=5432;Database=dragonflydb;Pooling=true;",
                o => o.UseTrigrams());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            if (builder == null)
            {
                return;
            }

            builder.Entity<PostTag>().Ignore(x => x.Post).HasKey(x => new { x.PostId, x.TagName });

            builder.Entity<Project>(p => {
                p.HasIndex(p => new { p.Name, p.UserId }).IsUnique();
            });


            builder.Entity<ProjectAdmin>()
                .HasKey(t => new { t.ProjectId, t.AdminId });

            builder.Entity<ProjectAdmin>()
               .HasOne(pa => pa.Project)
               .WithMany(a => a.Admins)
               .HasForeignKey(pa => pa.ProjectId);

            builder.Entity<ProjectAdmin>()
                .HasOne(pa => pa.Admin)
                .WithMany(p => p.AdminedProjects)
                .HasForeignKey(pa => pa.AdminId);



            builder.Entity<ProjectMaintainer>()
                .HasKey(t => new { t.ProjectId, t.MaintainerId });

            builder.Entity<ProjectMaintainer>()
               .HasOne(pm => pm.Project)
               .WithMany(a => a.Maintainers)
               .HasForeignKey(pm => pm.ProjectId);

            builder.Entity<ProjectMaintainer>()
                .HasOne(pm => pm.Maintainer)
                .WithMany(p => p.MaintainedProjects)
                .HasForeignKey(pm => pm.MaintainerId);



            builder.Entity<IssueIssueType>()
                .HasKey(t => new { t.IssueId, t.IssueTypeId });

            builder.Entity<IssueIssueType>()
               .HasOne(pi => pi.Issue)
               .WithMany(i => i.Types)
               .HasForeignKey(pi => pi.IssueId);

            builder.Entity<IssueIssueType>()
                .HasOne(pi => pi.IssueType)
                .WithMany(it => it.Issues)
                .HasForeignKey(pi => pi.IssueTypeId);
        }
    }
}