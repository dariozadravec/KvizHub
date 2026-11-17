using KvizHub.Api.Data.Converters;
using KvizHub.Api.Data.Entities;
using KvizHub.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KvizHub.Api.Data
{
    public class KvizHubDbContext(DbContextOptions<KvizHubDbContext> options)
    : IdentityDbContext<AppUser, IdentityRole<int>, int>(options)
    {
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
        public DbSet<Venue> Venues => Set<Venue>();
        public DbSet<QuizEvent> QuizEvents => Set<QuizEvent>();
        public DbSet<Section> Sections => Set<Section>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<SectionScore> SectionScores => Set<SectionScore>();
        public DbSet<Feedback> Feedbacks => Set<Feedback>();
        public DbSet<SectionFeedback> SectionFeedbacks => Set<SectionFeedback>();

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<UserRole>().HaveConversion<SnakeCaseEnumConverter<UserRole>>();
            configurationBuilder.Properties<EventStatus>().HaveConversion<SnakeCaseEnumConverter<EventStatus>>();
            configurationBuilder.Properties<ReservationStatus>().HaveConversion<SnakeCaseEnumConverter<ReservationStatus>>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(e =>
            {
                e.ToTable("app_user");
                e.HasIndex(u => u.NormalizedEmail).IsUnique();
            });

            builder.Entity<Team>().ToTable("team");

            builder.Entity<TeamMember>(e =>
            {
                e.ToTable("team_member");
                e.HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Venue>(e =>
            {
                e.ToTable("venue");
                e.HasIndex(v => v.NormalizedAddress).IsUnique();
                e.HasOne(v => v.CreatedBy).WithMany().HasForeignKey(v => v.CreatedById).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<QuizEvent>(e =>
            {
                e.ToTable("quiz_event");
                e.HasOne(q => q.Organizer).WithMany().HasForeignKey(q => q.OrganizerId).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(q => q.Venue).WithMany().HasForeignKey(q => q.VenueId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Section>(e =>
            {
                e.ToTable("section");
                e.HasIndex(s => new { s.EventId, s.Position }).IsUnique();
            });

            builder.Entity<Reservation>(e =>
            {
                e.ToTable("reservation");
                e.HasIndex(r => new { r.EventId, r.TeamId }).IsUnique();
                e.HasOne(r => r.Leader).WithMany().HasForeignKey(r => r.LeaderId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<SectionScore>(e =>
            {
                e.ToTable("section_score");
                e.HasIndex(s => new { s.ReservationId, s.SectionId }).IsUnique();
                e.Property(s => s.Points).HasPrecision(5, 1);
            });

            builder.Entity<Feedback>(e =>
            {
                e.ToTable("feedback");
                e.HasOne(f => f.Reservation).WithOne(r => r.Feedback).HasForeignKey<Feedback>(f => f.ReservationId);
            });

            builder.Entity<SectionFeedback>().ToTable("section_feedback");
        }
    }
}
