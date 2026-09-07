using System.ComponentModel.DataAnnotations;
using CompanyProfile.Core.Resources;

namespace CompanyProfile.Core.DTOs;

public record CompanyInfoResponseDto(
    string Language,
    string Name,
    string Description,
    string Vision,
    string Mission);

public record UpdateCompanyInfoDto(

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(150,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Name")]
    string Name,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(2000,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Description")]
    string Description,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(1000,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Vision")]
    string Vision,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(1000,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Mission")]
    string Mission);

public record ServiceResponseDto(
    int Id,
    string Language,
    string Title,
    string Description,
    string Icon);

public record CreateServiceDto(

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(150,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Title")]
    string Title,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(2000,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Description")]
    string Description,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(100,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Icon")]
    string Icon);

public record UpdateServiceDto(

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(150,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Title")]
    string Title,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(2000,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Description")]
    string Description,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(100,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Icon")]
    string Icon);

public record CreateServiceTranslationDto(

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(150,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Title")]
    string Title,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(2000,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Description")]
    string Description);

public record UpdateServiceTranslationDto(

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(150,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Title")]
    string Title,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(2000,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Description")]
    string Description);

public record CreateTeamMemberDto(

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(500,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "ImageUrl")]
    string ImageUrl);

public record UpdateTeamMemberDto(

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(500,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "ImageUrl")]
    string ImageUrl);

public record CreateTeamMemberTranslationDto(

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(150,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Name")]
    string Name,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(150,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Role")]
    string Role,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(2000,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Bio")]
    string Bio);

public record UpdateTeamMemberTranslationDto(

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(150,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Name")]
    string Name,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(150,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Role")]
    string Role,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(2000,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Bio")]
    string Bio);

public record CreateContactMessageDto(

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(150,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Name")]
    string Name,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: EmailAddress(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "EmailAddress")]
    [property: StringLength(255,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Email")]
    string Email,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(200,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Subject")]
    string Subject,

    [property: Required(
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "Required")]
    [property: StringLength(5000,
        ErrorMessageResourceType = typeof(ValidationResources),
        ErrorMessageResourceName = "StringLength")]
    [property: Display(
        ResourceType = typeof(ValidationResources),
        Name = "Message")]
    string Message);

public record ContactMessageResponseDto(
    int Id,
    string Message,
    DateTime CreatedAt);

public record ServiceWithTranslationsResponseDto(
    int Id,
    string Icon,
    List<ServiceTranslationResponseDto> Translations);

public record ServiceTranslationResponseDto(
    string Language,
    string Title,
    string Description);

public record CompanyInfoWithTranslationsResponseDto(
    List<CompanyInfoResponseDto> Translations);
