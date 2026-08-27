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

        publicGroup.MapGet("/info", async (AppDbContext db) =>
        {
            var info = await db.CompanyInformation.FirstOrDefaultAsync();
            return info is null ? Results.NotFound() : Results.Ok(new CompanyInfoResponseDto(info.Name, info.Description, info.Vision, info.Mission));
        });

        publicGroup.MapGet("/services", async (AppDbContext db) =>
        {
            var services = await db.Services.Select(s => new ServiceResponseDto(s.Id, s.Title, s.Description, s.Icon)).ToListAsync();
            return Results.Ok(services);
        });

        publicGroup.MapPost("/contact", async (CreateContactMessageDto inputDto, AppDbContext db) =>
        {
            var dbMessage = new ContactMessage { Name = inputDto.Name, Email = inputDto.Email, Subject = inputDto.Subject, Message = inputDto.Message, CreatedAt = DateTime.UtcNow };
            db.ContactMessages.Add(dbMessage);
            await db.SaveChangesAsync();
            return Results.Created($"/api/contact/{dbMessage.Id}", new ContactMessageResponseDto(dbMessage.Id, "تم استلام رسالتك.", dbMessage.CreatedAt));
        });

        // --------------------------------------------------------
        // مجموعات الإدارة (الخاصة بلوحة تحكم الشركة Dashboard)
        // --------------------------------------------------------
        var adminGroup = app.MapGroup("/api/admin").WithTags("Admin Dashboard API");
        adminGroup.MapPut("/info", async (UpdateCompanyInfoDto dto, AppDbContext db) =>
        {
            var info = await db.CompanyInformation.FirstOrDefaultAsync();
            if (info is null) return Results.NotFound("بيانات الشركة غير موجودة لتحديثها!");

            info.Name = dto.Name;
            info.Description = dto.Description;
            info.Vision = dto.Vision;
            info.Mission = dto.Mission;

            await db.SaveChangesAsync();
            return Results.Ok(new { Message = "تم تحديث معلومات الشركة بنجاح" });
        }).WithSummary("تعديل ملف الشركة التعريفي الرئيسي");

        adminGroup.MapPost("/services", async (CreateServiceDto dto, AppDbContext db) =>
        {
            var newService = new Service { Title = dto.Title, Description = dto.Description, Icon = dto.Icon };
            db.Services.Add(newService);
            await db.SaveChangesAsync();

            return Results.Created($"/api/services/{newService.Id}", new ServiceResponseDto(newService.Id, newService.Title, newService.Description, newService.Icon));
        }).WithSummary("إضافة خدمة جديدة قائمة خدمات الشركة");

        adminGroup.MapPut("/services/{id}", async (int id, UpdateServiceDto dto, AppDbContext db) =>
        {
            var service = await db.Services.FindAsync(id);
            if (service is null) return Results.NotFound("الخدمة غير موجودة لتعديلها!");

            service.Title = dto.Title;
            service.Description = dto.Description;
            service.Icon = dto.Icon;

            await db.SaveChangesAsync();
            return Results.NoContent();
        }).WithSummary("تعديل بيانات خدمة حالية عبر الـ ID");

        adminGroup.MapDelete("/services/{id}", async (int id, AppDbContext db) =>
        {
            var service = await db.Services.FindAsync(id);
            if (service is null) return Results.NotFound("الخدمة المطلوبة غير موجودة لحذفها!");

            db.Services.Remove(service);
            await db.SaveChangesAsync();
            return Results.Ok(new { Message = "تم حذف الخدمة بنجاح" });
        }).WithSummary("حذف خدمة نهائياً من قاعدة البيانات");
    }
}
