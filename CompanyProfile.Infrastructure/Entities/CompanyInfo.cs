namespace CompanyProfile.Infrastructure.Entities;

public class CompanyInfo
{
    public int Id { get; set; }

    public ICollection<CompanyInfoTranslation> Translations { get; set; } = new List<CompanyInfoTranslation>();
}
public class CompanyInfoTranslation
{
    public int Id { get; set; }

    public int CompanyInfoId { get; set; }

    public string Language { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Vision { get; set; } = string.Empty;

    public string Mission { get; set; } = string.Empty;

    public CompanyInfo CompanyInfo { get; set; } = null!;
}

public class Service
{
    public int Id { get; set; }

    public string Icon { get; set; } = string.Empty;

    public ICollection<ServiceTranslation> Translations { get; set; } = new List<ServiceTranslation>();
}
public class ServiceTranslation
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public string Language { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Service Service { get; set; } = null!;
}
public class TeamMember
{
    public int Id { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public ICollection<TeamMemberTranslation> Translations { get; set; }= new List<TeamMemberTranslation>();
}
public class TeamMemberTranslation
{
    public int Id { get; set; }

    public int TeamMemberId { get; set; }

    public string Language { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;

    public TeamMember TeamMember { get; set; } = null!;
}
public class ContactMessage
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}


