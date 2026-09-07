using System.Globalization;

namespace CompanyProfile.Api.Services;

public sealed class LanguageContext : ILanguageContext
{
    public string Language => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
}
