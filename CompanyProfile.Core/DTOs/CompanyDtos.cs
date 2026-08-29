using System.ComponentModel.DataAnnotations;

namespace CompanyProfile.Core.DTOs;

public record CompanyInfoResponseDto(
    string Name,
    string Description,
    string Vision,
    string Mission);

public record UpdateCompanyInfoDto(

    [property: Required(ErrorMessage = "Name is required.")]
    [property: StringLength(150, ErrorMessage = "Name cannot exceed 150 characters.")]
    string Name,

    [property: Required(ErrorMessage = "Description is required.")]
    [property: StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
    string Description,

    [property: Required(ErrorMessage = "Vision is required.")]
    [property: StringLength(1000, ErrorMessage = "Vision cannot exceed 1000 characters.")]
    string Vision,

    [property: Required(ErrorMessage = "Mission is required.")]
    [property: StringLength(1000, ErrorMessage = "Mission cannot exceed 1000 characters.")]
    string Mission);
public record ServiceResponseDto(
    int Id,
    string Title,
    string Description,
    string Icon);

public record CreateServiceDto(

    [property: Required(ErrorMessage = "Title is required.")]
    [property: StringLength(150, ErrorMessage = "Title cannot exceed 150 characters.")]
    string Title,

    [property: Required(ErrorMessage = "Description is required.")]
    [property: StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    string Description,

    [property: Required(ErrorMessage = "Icon is required.")]
    [property: StringLength(100, ErrorMessage = "Icon cannot exceed 100 characters.")]
    string Icon);

public record UpdateServiceDto(

    [property: Required(ErrorMessage = "Title is required.")]
    [property: StringLength(150, ErrorMessage = "Title cannot exceed 150 characters.")]
    string Title,

    [property: Required(ErrorMessage = "Description is required.")]
    [property: StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    string Description,

    [property: Required(ErrorMessage = "Icon is required.")]
    [property: StringLength(100, ErrorMessage = "Icon cannot exceed 100 characters.")]
    string Icon);


// ======================================================
// Contact
// ======================================================

public record CreateContactMessageDto(

    [property: Required(ErrorMessage = "Name is required.")]
    [property: StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    string Name,

    [property: Required(ErrorMessage = "Email is required.")]
    [property: EmailAddress(ErrorMessage = "Invalid email address.")]
    [property: StringLength(254, ErrorMessage = "Email cannot exceed 254 characters.")]
    string Email,

    [property: Required(ErrorMessage = "Subject is required.")]
    [property: StringLength(200, ErrorMessage = "Subject cannot exceed 200 characters.")]
    string Subject,

    [property: Required(ErrorMessage = "Message is required.")]
    [property: StringLength(2000, ErrorMessage = "Message cannot exceed 2000 characters.")]
    string Message);



public record ContactMessageResponseDto(
    int Id,
    string StatusMessage,
    DateTime ReceivedAt);

public record TeamMemberResponseDto(
    int Id,
    string Name,
    string Role,
    string Bio,
    string ImageUrl);

