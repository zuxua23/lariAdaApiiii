using Microsoft.IdentityModel.Tokens;
using InventoryControl.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenHelper
{
    private readonly IConfiguration _config;

    public JwtTokenHelper(IConfiguration config)
    {
        _config = config;
    }

    public Task<string> GenerateTokenAsync(
        User user,
        List<string> permissions,
        List<string> roles,
        int expireMinutes = 60)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Roles
        foreach (var role in roles.Distinct())
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Permissions
        foreach (var permission in permissions.Distinct())
        {
            claims.Add(new Claim("permission", permission));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        Console.WriteLine("===== DEBUG LOGIN =====");
        Console.WriteLine("UserId : " + user.UserId);

        foreach (var r in roles)
            Console.WriteLine("ROLE: " + r);

        foreach (var p in permissions)
            Console.WriteLine("PERMISSION: " + p);

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Task.FromResult(jwt);
    }
}