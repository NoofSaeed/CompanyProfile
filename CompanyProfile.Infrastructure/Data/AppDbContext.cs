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

        //// =========================
        //// Seed Data
        //// =========================
        //modelBuilder.Entity<CompanyInfo>().HasData(
        //    new CompanyInfo
        //    {
        //        Id = 1,
        //        Name = "تقنية الغد",
        //        Description = "شركة رائدة في الحلول البرمجية",
        //        Vision = "رؤيتنا قيادة التحول الرقمي",
        //        Mission = "رسالتنا تقديم برمجيات عالية الجودة"
        //    }
        //);

        //modelBuilder.Entity<Service>().HasData(
        //    new Service
        //    {
        //        Id = 1,
        //        Title = "تطوير الويب",
        //        Description = "بناء مواقع وتطبيقات ويب سريعة وآمنة",
        //        Icon = "web-icon"
        //    },
        //    new Service
        //    {
        //        Id = 2,
        //        Title = "تطوير تطبيقات الموبايل",
        //        Description = "تطبيقات هواتف ذكية لأنظمة iOS و Android",
        //        Icon = "mobile-icon"
        //    }
        //);
    }
}