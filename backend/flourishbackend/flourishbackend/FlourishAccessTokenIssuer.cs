using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace flourishbackend;

public interface IFlourishAccessTokenIssuer
{
    /// <summary>Null when JWT auth is disabled (no signing key).</summary>
    string? CreateAccessToken(Guid userId, string userType);
}

public sealed class FlourishAccessTokenIssuer : IFlourishAccessTokenIssuer
{
    private readonly SigningCredentials _creds;

    public FlourishAccessTokenIssuer(string signingKey)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        _creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }

    public string CreateAccessToken(Guid userId, string userType)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("user_type", userType),
        };
        var token = new JwtSecurityToken(
            issuer: "Flourish",
            audience: "Flourish",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(14),
            signingCredentials: _creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public sealed class NullFlourishAccessTokenIssuer : IFlourishAccessTokenIssuer
{
    public string? CreateAccessToken(Guid userId, string userType) => null;
}
