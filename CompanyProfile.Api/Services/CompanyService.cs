using CompanyProfile.Shared.DTOs;
using CompanyProfile.Infrastructure.Entities;
using CompanyProfile.Infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
namespace CompanyProfile.Api.Services;

public class CompanyService
{
    private readonly IApplicationDbContext _db;

    public CompanyService(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<CompanyInfoResponseDto?> GetCompanyInfoAsync(string language)
    {
        return await _db.CompanyInfoTranslations
            .AsNoTracking()
            .Where(t => t.Language == language)
            .ProjectToType<CompanyInfoResponseDto>()
            .FirstOrDefaultAsync();
    }

    public async Task<CompanyInfoWithTranslationsResponseDto?> GetCompanyInfoWithAllTransAsync()
    {
        var translations = await _db.CompanyInfoTranslations
            .AsNoTracking()
            .ProjectToType<CompanyInfoResponseDto>()
            .ToListAsync();

        if (translations.Count == 0) return null;

        return new CompanyInfoWithTranslationsResponseDto(translations);
    }

    public async Task<List<ServiceResponseDto>> GetServicesAsync(string language)
    {
        return await _db.ServiceTranslations
            .AsNoTracking()
            .Where(t => t.Language == language)
            .ProjectToType<ServiceResponseDto>()
            .ToListAsync();
    }

    public async Task<ServiceResponseDto?> GetServiceByIdAsync(int id, string language)
    {
        return await _db.ServiceTranslations
            .AsNoTracking()
            .Where(t => t.ServiceId == id && t.Language == language)
            .ProjectToType<ServiceResponseDto>()
            .FirstOrDefaultAsync();
    }

    public async Task<ServiceWithTranslationsResponseDto?> GetServiceByIdWithAllTransAsync(int id)
    {
        return await _db.Services
            .AsNoTracking()
            .Where(s => s.Id == id)
            .ProjectToType<ServiceWithTranslationsResponseDto>()
            .FirstOrDefaultAsync();
    }

    public async Task<ContactMessageResponseDto> CreateContactMessageAsync(CreateContactMessageDto dto)
    {
        var dbMessage = dto.Adapt<ContactMessage>();
        dbMessage.CreatedAt = DateTime.UtcNow;

        _db.ContactMessages.Add(dbMessage);
        await _db.SaveChangesAsync();

        return new ContactMessageResponseDto(dbMessage.Id, "تم استلام رسالتك.", dbMessage.CreatedAt);
    }

    public async Task<CompanyInfoResponseDto?> UpdateCompanyInfoAsync(string language, UpdateCompanyInfoDto dto)
    {
        var info = await _db.CompanyInformation
            .Include(x => x.Translations)
            .FirstOrDefaultAsync();

        if (info is null) return null;

        var translation = info.Translations.FirstOrDefault(t => t.Language == language);

        if (translation is null)
        {
            translation = new CompanyInfoTranslation
            {
                CompanyInfoId = info.Id,
                Language = language
            };
            info.Translations.Add(translation);
        }

        dto.Adapt(translation);
        await _db.SaveChangesAsync();

        return translation.Adapt<CompanyInfoResponseDto>();
    }

    public async Task<ServiceResponseDto> AddServiceAsync(string language, CreateServiceDto dto)
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

        _db.Services.Add(newService);
        await _db.SaveChangesAsync();

        return new ServiceResponseDto(newService.Id, language, dto.Title, dto.Description, dto.Icon);
    }

    public async Task<ServiceResponseDto?> UpdateServiceAsync(int id, string language, UpdateServiceDto dto)
    {
        var service = await _db.Services
            .Include(s => s.Translations)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (service is null) return null;

        service.Icon = dto.Icon;

        var translation = service.Translations.FirstOrDefault(t => t.Language == language);

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

        await _db.SaveChangesAsync();

        return new ServiceResponseDto(service.Id, language, translation.Title, translation.Description, service.Icon);
    }

    public async Task<bool> DeleteServiceAsync(int id)
    {
        var service = await _db.Services
            .Include(s => s.Translations)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (service is null) return false;

        _db.Services.Remove(service);
        await _db.SaveChangesAsync();
        return true;
    }
}