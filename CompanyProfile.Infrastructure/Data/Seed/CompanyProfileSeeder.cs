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

        dbContext.CompanyInformation.Add(new CompanyInfo
        {
            Id = 1,
            Name = "تقنية الغد",
            Description = "شركة رائدة في الحلول البرمجية",
            Vision = "رؤيتنا قيادة التحول الرقمي",
            Mission = "رسالتنا تقديم برمجيات عالية الجودة"
        });

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
                Title = "تطوير الويب",
                Description = "بناء مواقع وتطبيقات ويب سريعة وآمنة",
                Icon = "web-icon"
            },
            new Service
            {
                Id = 2,
                Title = "تطوير تطبيقات الموبايل",
                Description = "تطبيقات هواتف ذكية لأنظمة iOS و Android",
                Icon = "mobile-icon"
            });

        await dbContext.SaveChangesAsync();
    }
}