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
            .Select(t => new ServiceResponseDto(
                t.ServiceId,
                t.Language,
                t.Title,
                t.Description,
                t.Service.Icon))
            .ToListAsync();
    }

    public async Task<ServiceResponseDto?> GetServiceByIdAsync(int id, string language)
    {
        return await _db.ServiceTranslations
            .AsNoTracking()
            .Where(t => t.ServiceId == id && t.Language == language)
            .Select(t => new ServiceResponseDto(
                t.ServiceId,
                t.Language,
                t.Title,
                t.Description,
                t.Service.Icon))
            .FirstOrDefaultAsync();
    }

    public async Task<List<TeamMemberResponseDto>> GetTeamMembersAsync(string language)
    {
        return await _db.TeamMemberTranslations
            .AsNoTracking()
            .Where(t => t.Language == language)
            .Select(t => new TeamMemberResponseDto(
                t.TeamMemberId,
                t.TeamMember.ImageUrl,
                t.Language,
                t.Name,
                t.Role,
                t.Bio))
            .ToListAsync();
    }

    public async Task<TeamMemberResponseDto> AddTeamMemberAsync(
        string language,
        CreateTeamMemberAdminDto dto)
    {
        var member = new TeamMember
        {
            ImageUrl = dto.ImageUrl,
            Translations =
            [
                new TeamMemberTranslation
                {
                    Language = language,
                    Name = dto.Name,
                    Role = dto.Role,
                    Bio = dto.Bio
                }
            ]
        };

        _db.Team.Add(member);
        await _db.SaveChangesAsync();

        return new TeamMemberResponseDto(member.Id, member.ImageUrl, language, dto.Name, dto.Role, dto.Bio);
    }

    public async Task<TeamMemberResponseDto?> UpdateTeamMemberAsync(
        int id,
        string language,
        UpdateTeamMemberAdminDto dto)
    {
        var member = await _db.Team
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (member is null)
            return null;

        member.ImageUrl = dto.ImageUrl;
        var translation = member.Translations.FirstOrDefault(x => x.Language == language);

        if (translation is null)
        {
            translation = new TeamMemberTranslation { TeamMemberId = id, Language = language };
            member.Translations.Add(translation);
        }

        translation.Name = dto.Name;
        translation.Role = dto.Role;
        translation.Bio = dto.Bio;
        await _db.SaveChangesAsync();

        return new TeamMemberResponseDto(id, member.ImageUrl, language, translation.Name, translation.Role, translation.Bio);
    }

    public async Task<TeamMemberResponseDto?> AddTeamMemberTranslationAsync(
        int id,
        string language,
        CreateTeamMemberTranslationDto dto)
    {
        var member = await _db.Team
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (member is null || member.Translations.Any(x => x.Language == language))
            return null;

        var translation = new TeamMemberTranslation
        {
            TeamMemberId = id,
            Language = language,
            Name = dto.Name,
            Role = dto.Role,
            Bio = dto.Bio
        };

        member.Translations.Add(translation);
        await _db.SaveChangesAsync();
        return new TeamMemberResponseDto(id, member.ImageUrl, language, dto.Name, dto.Role, dto.Bio);
    }

    public async Task<bool> DeleteTeamMemberAsync(int id)
    {
        var member = await _db.Team.FirstOrDefaultAsync(x => x.Id == id);
        if (member is null)
            return false;

        _db.Team.Remove(member);
        await _db.SaveChangesAsync();
        return true;
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

    public async Task<List<ContactMessageAdminDto>> GetContactMessagesAsync()
    {
        return await _db.ContactMessages
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ContactMessageAdminDto(x.Id, x.Name, x.Email, x.Subject, x.Message, x.CreatedAt))
            .ToListAsync();
    }

    public async Task<bool> DeleteContactMessageAsync(int id)
    {
        var message = await _db.ContactMessages.FirstOrDefaultAsync(x => x.Id == id);
        if (message is null)
            return false;

        _db.ContactMessages.Remove(message);
        await _db.SaveChangesAsync();
        return true;
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

    public async Task<ServiceResponseDto?> AddServiceTranslationAsync(
        int id,
        string language,
        CreateServiceTranslationDto dto)
    {
        var service = await _db.Services
            .Include(x => x.Translations)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (service is null || service.Translations.Any(x => x.Language == language))
            return null;

        var translation = new ServiceTranslation
        {
            ServiceId = id,
            Language = language,
            Title = dto.Title,
            Description = dto.Description
        };

        service.Translations.Add(translation);
        await _db.SaveChangesAsync();

        return new ServiceResponseDto(id, language, translation.Title, translation.Description, service.Icon);
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