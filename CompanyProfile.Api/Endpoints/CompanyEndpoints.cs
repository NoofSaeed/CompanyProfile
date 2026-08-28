using CompanyProfile.Core.DTOs;
using CompanyProfile.Core.Entities;
using CompanyProfile.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CompanyProfile.API.Endpoints;

public static class CompanyEndpoints
{
    public static void MapCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        // --------------------------------------------------------
        // المجموعات العامة (المتاحة للزوار والـ Frontend العام)
        // --------------------------------------------------------
        var publicGroup = app.MapGroup("/api").WithTags("Public Website API");

        publicGroup.MapGet("/info", GetCompanyInfo)
                   .WithSummary("جلب معلومات الشركة الأساسية للـ Frontend");

        publicGroup.MapGet("/services", GetServices)
                   .WithSummary("جلب خدمات الشركة المقسمة للـ Frontend");

        publicGroup.MapPost("/contact", CreateContactMessage)
                   .WithSummary("إرسال رسالة تواصل جديدة من الـ Frontend");

        // --------------------------------------------------------
        // مجموعات الإدارة (الخاصة بلوحة تحكم الشركة Dashboard)
        // --------------------------------------------------------
        var adminGroup = app.MapGroup("/api/admin").WithTags("Admin Dashboard API");

        adminGroup.MapPut("/info", UpdateCompanyInfo)
                  .WithSummary("تعديل ملف الشركة التعريفي الرئيسي");

        adminGroup.MapPost("/services", AddService)
                  .WithSummary("إضافة خدمة جديدة قائمة خدمات الشركة");

        adminGroup.MapPut("/services/{id}", UpdateService)
                  .WithSummary("تعديل بيانات خدمة حالية عبر الـ ID");

        adminGroup.MapDelete("/services/{id}", DeleteService)
                  .WithSummary("حذف خدمة نهائياً من قاعدة البيانات");
    }

    private static async Task<IResult> GetCompanyInfo(AppDbContext db)
    {
        var info = await db.CompanyInformation.FirstOrDefaultAsync();
        return info is null
            ? Results.NotFound()
            : Results.Ok(new CompanyInfoResponseDto(info.Name, info.Description, info.Vision, info.Mission));
    }

    private static async Task<IResult> GetServices(AppDbContext db)
    {
        var services = await db.Services
            .Select(s => new ServiceResponseDto(s.Id, s.Title, s.Description, s.Icon))
            .ToListAsync();
        return Results.Ok(services);
    }

    private static async Task<IResult> CreateContactMessage(CreateContactMessageDto inputDto, AppDbContext db)
    {
        var dbMessage = new ContactMessage
        {
            Name = inputDto.Name,
            Email = inputDto.Email,
            Subject = inputDto.Subject,
            Message = inputDto.Message,
            CreatedAt = DateTime.UtcNow
        };

        db.ContactMessages.Add(dbMessage);
        await db.SaveChangesAsync();

        return Results.Created($"/api/contact/{dbMessage.Id}", new ContactMessageResponseDto(dbMessage.Id, "تم استلام رسالتك.", dbMessage.CreatedAt));
    }

    private static async Task<IResult> UpdateCompanyInfo(UpdateCompanyInfoDto dto, AppDbContext db)
    {
        var info = await db.CompanyInformation.FirstOrDefaultAsync();
        if (info is null) return Results.NotFound("بيانات الشركة غير موجودة لتحديثها!");

        info.Name = dto.Name;
        info.Description = dto.Description;
        info.Vision = dto.Vision;
        info.Mission = dto.Mission;

        await db.SaveChangesAsync();
        return Results.Ok(new { Message = "تم تحديث معلومات الشركة بنجاح" });
    }

    private static async Task<IResult> AddService(CreateServiceDto dto, AppDbContext db)
    {
        var newService = new Service { Title = dto.Title, Description = dto.Description, Icon = dto.Icon };
        db.Services.Add(newService);
        await db.SaveChangesAsync();

        return Results.Created($"/api/services/{newService.Id}", new ServiceResponseDto(newService.Id, newService.Title, newService.Description, newService.Icon));
    }

    private static async Task<IResult> UpdateService(int id, UpdateServiceDto dto, AppDbContext db)
    {
        var service = await db.Services.FindAsync(id);
        if (service is null) return Results.NotFound("الخدمة غير موجودة لتعديلها!");

        service.Title = dto.Title;
        service.Description = dto.Description;
        service.Icon = dto.Icon;

        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteService(int id, AppDbContext db)
    {
        var service = await db.Services.FindAsync(id);
        if (service is null) return Results.NotFound("الخدمة المطلوبة غير موجودة لحذفها!");

        db.Services.Remove(service);
        await db.SaveChangesAsync();
        return Results.Ok(new { Message = "تم حذف الخدمة بنجاح" });
    }
}
