namespace CompanyProfile.Core.DTOs;

public record CompanyInfoResponseDto(string Name, string Description, string Vision, string Mission);
public record ServiceResponseDto(int Id, string Title, string Description, string Icon);
public record TeamMemberResponseDto(int Id, string Name, string Role, string Bio, string ImageUrl);
public record CreateContactMessageDto(string Name, string Email, string Subject, string Message);
public record ContactMessageResponseDto(int Id, string StatusMessage, DateTime ReceivedAt);
public record UpdateCompanyInfoDto(string Name, string Description, string Vision, string Mission);
public record CreateServiceDto(string Title, string Description, string Icon);
public record UpdateServiceDto(string Title, string Description, string Icon);
