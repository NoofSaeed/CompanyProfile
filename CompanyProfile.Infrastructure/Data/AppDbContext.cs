using CompanyProfile.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyProfile.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<CompanyInfo> CompanyInformation => Set<CompanyInfo>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<TeamMember> Team => Set<TeamMember>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<CompanyInfo>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(x => x.Vision)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.Mission)
                .IsRequired()
                .HasMaxLength(1000);
        });
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(x => x.Icon)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<TeamMember>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Role)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Bio)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);
        });
        modelBuilder.Entity<ContactMessage>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(254);

            entity.Property(x => x.Subject)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(x => x.CreatedAt)
                .IsRequired();
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.UserId)
                .IsRequired();

            entity.Property(x => x.SessionHash)
                .IsRequired();

            entity.Property(x => x.IpAddress)
                .HasMaxLength(45);

            entity.Property(x => x.UserAgent)
                .HasMaxLength(1000);

            entity.Property(x => x.UserAgentHash)
                .HasMaxLength(64);

            entity.Property(x => x.CreatedAtUtc)
                .IsRequired();

            entity.Property(x => x.LastAccessedAtUtc)
                .IsRequired();

            entity.Property(x => x.ExpiresAtUtc)
                .IsRequired();

            entity.Property(x => x.AbsoluteExpiresAtUtc)
                .IsRequired();

            entity.Property(x => x.IsRevoked)
                .IsRequired();

            entity.HasIndex(x => x.SessionHash)
                .IsUnique();

            entity.HasIndex(x => x.ExpiresAtUtc);

            entity.HasIndex(x => x.UserId);
        });
        }

}