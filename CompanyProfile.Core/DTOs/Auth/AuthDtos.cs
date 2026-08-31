using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyProfile.Core.DTOs.Auth
{
    public sealed record LoginRequest(string UserName,string Password);

    public sealed record LoginResponse(string AccessToken,DateTime ExpiresAtUtc);
}
