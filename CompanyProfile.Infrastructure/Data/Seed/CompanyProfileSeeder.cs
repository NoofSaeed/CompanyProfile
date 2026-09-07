using CompanyProfile.Core.Entities;
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
}