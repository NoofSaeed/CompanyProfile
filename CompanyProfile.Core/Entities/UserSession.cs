namespace CompanyProfile.Core.Entities;

public class UserSession
{
    public long Id { get; set; }

    public string UserId { get; set; } = null!;

    public string SessionHash { get; set; } = null!;

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public string? UserAgentHash { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime LastAccessedAtUtc { get; set; }

    public DateTime ExpiresAtUtc { get; set; }

    public DateTime AbsoluteExpiresAtUtc { get; set; }

    public bool IsRevoked { get; set; }
}