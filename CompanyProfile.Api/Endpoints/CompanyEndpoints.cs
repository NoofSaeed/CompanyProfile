using CompanyProfile.Api.Filters;
using CompanyProfile.Api.Services;
using CompanyProfile.Core.DTOs;
using CompanyProfile.Core.Entities;
using CompanyProfile.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CompanyProfile.Api.Endpoints;

public static class CompanyEndpoints
{
    public static void MapCompanyEndpoints(this IEndpointRouteBuilder app)
    {
        var publicGroup = app.MapGroup("/api")
            .WithTags("Public Website API");

        publicGroup.MapGet("/info", GetCompanyInfo)
            .WithSummary("جلب معلومات الشركة باللغة المطلوبة");

        publicGroup.MapGet("/services", GetServices)
            .WithSummary("جلب خدمات الشركة باللغة المطلوبة");

        publicGroup.MapGet("/services/{id}", GetServiceById)
            .WithSummary("جلب خدمة الشركة باللغة المطلوبة");

        publicGroup.MapPost("/contact", CreateContactMessage)
            .AddEndpointFilter<ValidationFilter<CreateContactMessageDto>>()
            .WithSummary("إرسال رسالة تواصل جديدة");

        var adminGroup = app.MapGroup("/api/admin")
            .RequireAuthorization(policy =>
                policy.RequireRole("Admin"))
            .WithTags("Admin Dashboard API");

        adminGroup.MapGet("/servicesWithAllTrans/{id}", GetServiceByIdِWithAllTrans)
          .WithSummary("جلب خدمة معينة مع جميع الترجمات");

        adminGroup.MapGet("/infoWithTran", GetCompanyInfoWithAllTrans)
             .WithSummary("جلب معلومات الشركة بجميع اللغات");

        adminGroup.MapPut("/{language}/info",UpdateCompanyInfo)
            .AddEndpointFilter<ValidationFilter<UpdateCompanyInfoDto>>()
            .WithSummary("تعديل معلومات الشركة باللغة المحددة");

        adminGroup.MapPost("/{language}/services", AddService)
            .AddEndpointFilter<ValidationFilter<CreateServiceDto>>()
            .WithSummary("إضافة خدمة باللغة المحددة");

        adminGroup.MapPut("/{language}/services/{id}", UpdateService)
            .AddEndpointFilter<ValidationFilter<UpdateServiceDto>>()
            .WithSummary("تعديل خدمة باللغة المحددة");

        adminGroup.MapDelete("/services/{id}", DeleteService)
                .WithSummary("حذف خدمة نهائياً من قاعدة البيانات");

    }


    private static async Task<IResult> GetCompanyInfo(
        AppDbContext db,
        ILanguageContext languageContext)
    {
        var language = languageContext.Language;

        var translation = await db.CompanyInformation
            .AsNoTracking()
            .SelectMany(info => info.Translations)
            .FirstOrDefaultAsync(t =>
                t.Language == language);

        if (translation is null)
            return Results.NotFound();

        return Results.Ok(
            new CompanyInfoResponseDto(
                translation.Language,
                translation.Name,
                translation.Description,
                translation.Vision,
                translation.Mission));
    }
    private static async Task<IResult> GetCompanyInfoWithAllTrans(AppDbContext db)
    {
        var translations = await db.CompanyInformation
            .AsNoTracking()
            .SelectMany(info => info.Translations)
            .Select(t => new CompanyInfoResponseDto(
                t.Language,
                t.Name,
                t.Description,
                t.Vision,
                t.Mission))
            .ToListAsync();

        if (translations.Count == 0)
            return Results.NotFound();

        return Results.Ok(new CompanyInfoWithTranslationsResponseDto(translations));
    }


    private static async Task<IResult> GetServices(AppDbContext db,ILanguageContext languageContext)
    {
        var language = languageContext.Language;

        var services = await db.ServiceTranslations
            .AsNoTracking()
            .Where(t => t.Language == language)
            .Select(t => new ServiceResponseDto(
                t.ServiceId,
                t.Language,
                t.Title,
                t.Description,
                t.Service.Icon))
            .ToListAsync();

        return Results.Ok(services);
    }

    private static async Task<IResult> GetServiceById(int id, AppDbContext db, ILanguageContext languageContext)
    {
        var language = languageContext.Language;

        var service = await db.ServiceTranslations
            .AsNoTracking()
            .Where(t =>
                t.ServiceId == id &&
                t.Language == language)
            .Select(t => new ServiceResponseDto(
                t.ServiceId,
                t.Language,
                t.Title,
                t.Description,
                t.Service.Icon))
            .FirstOrDefaultAsync();

        if (service is null)
            return Results.NotFound("الخدمة غير موجودة.");

        return Results.Ok(service);
    }

    private static async Task<IResult> GetServiceByIdِWithAllTrans(int id,AppDbContext db)
    {
        var service = await db.Services
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new ServiceWithTranslationsResponseDto(
                s.Id,
                s.Icon,
                s.Translations
                    .Select(t => new ServiceTranslationResponseDto(
                        t.Language,
                        t.Title,
                        t.Description))
                    .ToList()
            ))
            .FirstOrDefaultAsync();

        if (service is null)
            return Results.NotFound("الخدمة غير موجودة.");

        return Results.Ok(service);
    }
   
    private static async Task<IResult> CreateContactMessage(CreateContactMessageDto inputDto,AppDbContext db)
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

        return Results.Created(
            $"/api/contact/{dbMessage.Id}",
            new ContactMessageResponseDto(
                dbMessage.Id,
                "تم استلام رسالتك.",
                dbMessage.CreatedAt));
    }

    private static async Task<IResult> UpdateCompanyInfo(string language,UpdateCompanyInfoDto dto,AppDbContext db)
    {
        var info = await db.CompanyInformation
            .Include(x => x.Translations)
            .FirstOrDefaultAsync();

        if (info is null)
            return Results.NotFound("بيانات الشركة غير موجودة.");

        var translation = info.Translations
            .FirstOrDefault(t => t.Language == language);

        if (translation is null)
        {
            translation = new CompanyInfoTranslation
            {
                CompanyInfoId = info.Id,
                Language = language
            };

            info.Translations.Add(translation);
        }

        translation.Name = dto.Name;
        translation.Description = dto.Description;
        translation.Vision = dto.Vision;
        translation.Mission = dto.Mission;

        await db.SaveChangesAsync();

        return Results.Ok(
            new CompanyInfoResponseDto(
                translation.Language,
                translation.Name,
                translation.Description,
                translation.Vision,
                translation.Mission));
    }

    private static async Task<IResult> AddService(string language,CreateServiceDto dto,AppDbContext db)
    {
        var newService = new Service
        {
            Icon = dto.Icon,
            Translations =
            [
                new ServiceTranslation
            {
                Language = language,
                Title = dto.Title,
                Description = dto.Description
            }
            ]
        };

        db.Services.Add(newService);

        await db.SaveChangesAsync();

        return Results.Created(
            $"/api/services/{newService.Id}",
            new ServiceResponseDto(
                newService.Id,
                language,
                dto.Title,
                dto.Description,
                dto.Icon));
    }

    private static async Task<IResult> UpdateService(int id,string language,UpdateServiceDto dto,AppDbContext db)
    {
        var service = await db.Services
            .Include(s => s.Translations)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (service is null)
            return Results.NotFound("الخدمة غير موجودة.");

        service.Icon = dto.Icon;

        var translation = service.Translations
            .FirstOrDefault(t => t.Language == language);

        if (translation is null)
        {
            translation = new ServiceTranslation
            {
                ServiceId = service.Id,
                Language = language
            };

            service.Translations.Add(translation);
        }

        translation.Title = dto.Title;
        translation.Description = dto.Description;

        await db.SaveChangesAsync();

        return Results.Ok(
            new ServiceResponseDto(
                service.Id,
                language,
                translation.Title,
                translation.Description,
                service.Icon));
    }

   
    private static async Task<IResult> DeleteService(int id,AppDbContext db)
    {
        var service = await db.Services
            .Include(s => s.Translations)
            .FirstOrDefaultAsync(s =>s.Id == id);

        if (service is null)
            return Results.NotFound("الخدمة المطلوبة غير موجودة.");

        db.Services.Remove(service);

        await db.SaveChangesAsync();

        return Results.Ok(new
        {
            Message = "تم حذف الخدمة وترجماتها بنجاح."
        });
    }
}
