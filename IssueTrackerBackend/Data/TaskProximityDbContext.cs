using Microsoft.EntityFrameworkCore;
using TaskProximity.Models;

namespace TaskProximity.Data
{
    public class TaskProximityDbContext : DbContext
    {
        public TaskProximityDbContext(DbContextOptions<TaskProximityDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<EmailNotification> EmailNotifications { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProject>()
                .HasKey(up => new { up.UserId, up.ProjectId });

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.Project)
                .WithMany(p => p.UserProjects)
                .HasForeignKey(up => up.ProjectId);

            modelBuilder.Entity<UserTeam>()
           .HasOne(ut => ut.Team)
           .WithMany(t => t.UserTeams)
           .HasForeignKey(ut => ut.TeamId);

            modelBuilder.Entity<UserTeam>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTeams)
                .HasForeignKey(ut => ut.UserId);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.Team)
                .WithMany(t => t.Invitations)
                .HasForeignKey(i => i.TeamId);

            modelBuilder.Entity<Invitation>()
                .HasOne(i => i.InvitedUser)
                .WithMany(u => u.Invitations)
                .HasForeignKey(i => i.InvitedUserId);
        }
    }
}