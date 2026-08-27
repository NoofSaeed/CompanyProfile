using CompanyProfile.Core.Entities; 
using Microsoft.EntityFrameworkCore;

namespace CompanyProfile.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CompanyInfo> CompanyInformation => Set<CompanyInfo>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<TeamMember> Team => Set<TeamMember>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompanyInfo>().HasData(
            new CompanyInfo { Id = 1, Name = "تقنية الغد", Description = "شركة رائدة في الحلول البرمجية", Vision = "رؤيتنا قيادة التحول الرقمي", Mission = "رسالتنا تقديم برمجيات عالية الجودة" }
        );

        modelBuilder.Entity<Service>().HasData(
            new Service { Id = 1, Title = "تطوير الويب", Description = "بناء مواقع وتطبيقات ويب سريعة وآمنة", Icon = "web-icon" },
            new Service { Id = 2, Title = "تطوير تطبيقات الموبايل", Description = "تطبيقات هواتف ذكية لأنظمة iOS و Android", Icon = "mobile-icon" }
        );
    }
}
