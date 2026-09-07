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
    public DbSet<CompanyInfoTranslation> CompanyInfoTranslations => Set<CompanyInfoTranslation>();

    public DbSet<Service> Services => Set<Service>();
    public DbSet<ServiceTranslation> ServiceTranslations => Set<ServiceTranslation>();

    public DbSet<UserSession> UserSessions => Set<UserSession>();

    public DbSet<TeamMember> Team => Set<TeamMember>();
    public DbSet<TeamMemberTranslation> TeamMemberTranslations => Set<TeamMemberTranslation>();

    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CompanyInfo>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasMany(x => x.Translations)
                .WithOne(x => x.CompanyInfo)
                .HasForeignKey(x => x.CompanyInfoId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CompanyInfoTranslation>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Language)
                .IsRequired()
                .HasMaxLength(2);

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

            entity.HasIndex(x => new
            {
                x.CompanyInfoId,
                x.Language
            })
            .IsUnique();
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Icon)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasMany(x => x.Translations)
                .WithOne(x => x.Service)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ServiceTranslation>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Language)
                .IsRequired()
                .HasMaxLength(2);

            entity.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1000);

            entity.HasIndex(x => new
            {
                x.ServiceId,
                x.Language
            })
            .IsUnique();
        });

        modelBuilder.Entity<TeamMember>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            entity.HasMany(x => x.Translations)
                .WithOne(x => x.TeamMember)
                .HasForeignKey(x => x.TeamMemberId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TeamMemberTranslation>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Language)
                .IsRequired()
                .HasMaxLength(2);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Role)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Bio)
                .IsRequired()
                .HasMaxLength(2000);

            entity.HasIndex(x => new
            {
                x.TeamMemberId,
                x.Language
            })
            .IsUnique();
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
