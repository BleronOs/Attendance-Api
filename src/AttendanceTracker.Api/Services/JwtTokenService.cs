using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AttendanceTracker.Api.Authentication;
using AttendanceTracker.Api.Configuration;
using AttendanceTracker.Api.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace AttendanceTracker.Api.Services;

public class JwtTokenService : ITokenService
{
    public static Dictionary<string, string> RefreshTokenDict;
    private readonly TokenConfiguration _tokenConfig;

    public JwtTokenService(TokenConfiguration tokenConfig)
    {
        RefreshTokenDict = new Dictionary<string, string>();
        _tokenConfig = tokenConfig;
    }

    public JwtToken GenerateToken(string userId, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenConfig.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {new Claim("id", userId), new Claim("role", role)}),
            Expires = DateTime.Now.AddDays((int)_tokenConfig.ExpirationInDays),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var createdToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(createdToken);

        var jwtToken = new JwtToken
        {
            Token = token,
            RefreshToken = GenerateRefreshToken(),
            ExpirationDate = tokenDescriptor.Expires,
        };

        return jwtToken;
    }

    private string GenerateRefreshToken(int size = 32)
    {
        var refreshToken = new byte[size];
        using var randomNumber = RandomNumberGenerator.Create();
        randomNumber.GetBytes(refreshToken);
        return Convert.ToBase64String(refreshToken);
    }
}