namespace CompanyProfile.Api;

public class AppSettings
{
    public SessionSettings Session { get; set; } = new();
}

public class SessionSettings
{
    public string CookieName { get; set; } = "__Host-user_session";

    public TimeSpan SessionLifetimeDays { get; set; } = TimeSpan.FromDays(14);

    public TimeSpan RenewalThresholdDays { get; set; } = TimeSpan.FromDays(1);

    public TimeSpan AbsoluteSessionLifetimeDays { get; set; } = TimeSpan.FromDays(60);
}