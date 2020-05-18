using DragonflyTracker.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Npgsql.NameTranslation;
using System;

namespace DragonflyTracker.Data
{
    public class PgMainDataContext : IdentityDbContext<DragonflyUser, IdentityRole<Guid>, Guid>
    {
        public PgMainDataContext(DbContextOptions<PgMainDataContext> options) : base(options)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (modelBuilder == null)
            {
                return;
            }

            var mapper = new NpgsqlSnakeCaseNameTranslator();
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    modelBuilder.Entity(entity.Name).Property(property.Name).HasColumnName(mapper.TranslateMemberName(property.Name)); //.ToTable(mapper.TranslateMemberName(entity.Name));
                    //property.Relational().ColumnName = mapper.TranslateMemberName(property.Name);
                }

                //entity.Relational().TableName = mapper.TranslateMemberName(entity.Relational().TableName);
                modelBuilder.Entity(entity.Name).ToTable(mapper.TranslateMemberName(entity.GetTableName()));
            }

            modelBuilder.Entity<Project>(p =>
            {
                p.HasOne(p => p.Creator).WithMany(du => du.CreatedProjects);
                p.HasOne(p => p.Owner).WithMany(du => du.OwnedProjects);
                p.HasIndex(p => new { p.Name, p.CreatorId }).IsUnique();
            });

            modelBuilder.Entity<PostTag>().Ignore(x => x.Post).HasKey(x => new { x.PostId, x.TagName });

            modelBuilder.Entity<ProjectAdmin>(pa =>
            {
                pa.HasKey(t => new { t.ProjectId, t.AdminId });
                pa.HasOne(pa => pa.Project).WithMany(a => a.Admins).HasForeignKey(pa => pa.ProjectId);
                pa.HasOne(pa => pa.Admin).WithMany(p => p.AdminedProjects).HasForeignKey(pa => pa.AdminId);
            });

            modelBuilder.Entity<ProjectMaintainer>(pm =>
            {
                pm.HasKey(t => new { t.ProjectId, t.MaintainerId });
                pm.HasOne(pm => pm.Project).WithMany(a => a.Maintainers).HasForeignKey(pm => pm.ProjectId);
                pm.HasOne(pm => pm.Maintainer).WithMany(p => p.MaintainedProjects).HasForeignKey(pm => pm.MaintainerId);
            });
            modelBuilder.Entity<IssueIssueType>(iit =>
            {
                iit.HasKey(t => new { t.IssueId, t.IssueTypeId });
                iit.HasOne(pi => pi.Issue).WithMany(i => i.Types).HasForeignKey(pi => pi.IssueId);
                iit.HasOne(pi => pi.IssueType).WithMany(it => it.Issues).HasForeignKey(pi => pi.IssueTypeId);
            });
        }
    }
}