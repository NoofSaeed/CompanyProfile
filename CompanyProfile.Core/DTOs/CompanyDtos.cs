using System.ComponentModel.DataAnnotations;

namespace CompanyProfile.Core.DTOs;


public record CompanyInfoResponseDto(
    string Language,
    string Name,
    string Description,
    string Vision,
    string Mission);

public record UpdateCompanyInfoDto(

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        150,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Name")]
    string Name,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        2000,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Description")]
    string Description,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        1000,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Vision")]
    string Vision,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        1000,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Mission")]
    string Mission);

public record ServiceResponseDto(
    int Id,
    string Language,
    string Title,
    string Description,
    string Icon);

public record CreateServiceDto(

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        150,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Title")]
    string Title,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        2000,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Description")]
    string Description,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        100,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Icon")]
    string Icon);

public record UpdateServiceDto(

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        150,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Title")]
    string Title,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        2000,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Description")]
    string Description,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        100,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Icon")]
    string Icon);


public record CreateServiceTranslationDto(

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        150,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Title")]
    string Title,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        2000,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Description")]
    string Description);

public record UpdateServiceTranslationDto(

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        150,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Title")]
    string Title,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        2000,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Description")]
    string Description);

public record CreateTeamMemberDto(

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        500,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.ImageUrl")]
    string ImageUrl);

public record UpdateTeamMemberDto(

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        500,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.ImageUrl")]
    string ImageUrl);

public record CreateTeamMemberTranslationDto(

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        150,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Name")]
    string Name,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        150,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Role")]
    string Role,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        2000,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Bio")]
    string Bio);

public record UpdateTeamMemberTranslationDto(

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        150,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Name")]
    string Name,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        150,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Role")]
    string Role,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        2000,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Bio")]
    string Bio);


public record CreateContactMessageDto(

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        150,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Name")]
    string Name,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: EmailAddress(
        ErrorMessage = "Validation.EmailAddress")]
    [property: StringLength(
        255,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Email")]
    string Email,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        200,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Subject")]
    string Subject,

    [property: Required(ErrorMessage = "Validation.Required")]
    [property: StringLength(
        5000,
        ErrorMessage = "Validation.StringLength")]
    [property: Display(Name = "Validation.Message")]
    string Message);

public record ContactMessageResponseDto(
    int Id,
    string Message,
    DateTime CreatedAt);
public record ServiceWithTranslationsResponseDto(
    int Id,
    string Icon,
    List<ServiceTranslationResponseDto> Translations
);

public record ServiceTranslationResponseDto(
    string Language,
    string Title,
    string Description
);
public record CompanyInfoWithTranslationsResponseDto(List<CompanyInfoResponseDto> Translations);