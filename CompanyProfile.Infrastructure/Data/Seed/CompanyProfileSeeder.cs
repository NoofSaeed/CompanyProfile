using CompanyProfile.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyProfile.Infrastructure.Data;

public static class CompanyProfileSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var dbContext = services.GetRequiredService<AppDbContext>();

        await SeedCompanyInfoAsync(dbContext);
        await SeedServicesAsync(dbContext);
        await SeedTeamAsync(dbContext);
    }

    private static async Task SeedCompanyInfoAsync(AppDbContext dbContext)
    {
        if (await dbContext.CompanyInformation.AnyAsync())
            return;

        var companyInfo = new CompanyInfo
        {
            Id = 1,
            Translations =
            [
                new CompanyInfoTranslation
                {
                    Language = "ar",
                    Name = "تقنية الغد",
                    Description = "شركة رائدة في الحلول البرمجية",
                    Vision = "رؤيتنا قيادة التحول الرقمي",
                    Mission = "رسالتنا تقديم برمجيات عالية الجودة"
                },
                new CompanyInfoTranslation
                {
                    Language = "en",
                    Name = "Tomorrow Technology",
                    Description = "A leading company in software solutions",
                    Vision = "Our vision is to lead digital transformation",
                    Mission = "Our mission is to deliver high-quality software"
                }
            ]
        };

        dbContext.CompanyInformation.Add(companyInfo);

        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedServicesAsync(AppDbContext dbContext)
    {
        if (await dbContext.Services.AnyAsync())
            return;

        dbContext.Services.AddRange(
            new Service
            {
                Id = 1,
                Icon = "web-icon",
                Translations =
                [
                    new ServiceTranslation
                    {
                        Language = "ar",
                        Title = "تطوير الويب",
                        Description = "بناء مواقع وتطبيقات ويب سريعة وآمنة"
                    },
                    new ServiceTranslation
                    {
                        Language = "en",
                        Title = "Web Development",
                        Description = "Building fast and secure websites and web applications"
                    }
                ]
            },
            new Service
            {
                Id = 2,
                Icon = "mobile-icon",
                Translations =
                [
                    new ServiceTranslation
                    {
                        Language = "ar",
                        Title = "تطوير تطبيقات الموبايل",
                        Description = "تطبيقات هواتف ذكية لأنظمة iOS و Android"
                    },
                    new ServiceTranslation
                    {
                        Language = "en",
                        Title = "Mobile App Development",
                        Description = "Smartphone applications for iOS and Android"
                    }
                ]
            });

        await dbContext.SaveChangesAsync();
    }

    private static async Task SeedTeamAsync(AppDbContext dbContext)
    {
        if (await dbContext.Team.AnyAsync())
            return;

        dbContext.Team.AddRange(
            new TeamMember
            {
                Id = 1,
                ImageUrl = "https://images.unsplash.com/photo-1560250097-0b93528c311a?auto=format&fit=crop&w=700&q=85",
                Translations =
                [
                    new TeamMemberTranslation { Language = "ar", Name = "أحمد العلي", Role = "المدير التنفيذي", Bio = "يقود الرؤية والاستراتيجية لبناء منتجات رقمية ذات أثر." },
                    new TeamMemberTranslation { Language = "en", Name = "Ahmed Al Ali", Role = "Chief Executive Officer", Bio = "Leads the vision and strategy behind meaningful digital products." }
                ]
            },
            new TeamMember
            {
                Id = 2,
                ImageUrl = "https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?auto=format&fit=crop&w=700&q=85",
                Translations =
                [
                    new TeamMemberTranslation { Language = "ar", Name = "سارة منصور", Role = "قائدة التصميم", Bio = "تحول الأفكار المعقدة إلى تجارب بسيطة وجميلة." },
                    new TeamMemberTranslation { Language = "en", Name = "Sarah Mansour", Role = "Design Lead", Bio = "Turns complex ideas into simple, beautiful experiences." }
                ]
            },
            new TeamMember
            {
                Id = 3,
                ImageUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?auto=format&fit=crop&w=700&q=85",
                Translations =
                [
                    new TeamMemberTranslation { Language = "ar", Name = "خالد ناصر", Role = "مهندس برمجيات", Bio = "يبني أنظمة مرنة وسريعة تجعل الطموح قابلًا للتنفيذ." },
                    new TeamMemberTranslation { Language = "en", Name = "Khaled Nasser", Role = "Software Engineer", Bio = "Builds resilient, fast systems that turn ambition into reality." }
                ]
            });

        await dbContext.SaveChangesAsync();
    }
}