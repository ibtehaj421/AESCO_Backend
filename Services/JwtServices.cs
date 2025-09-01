using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ASCO.Models;

public class JwtServices {
    private readonly IConfiguration _configuration;
    public JwtServices(IConfiguration configuration) {
        _configuration = configuration;
    }

    public string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"];
        var jwtIssuer = _configuration["Jwt:Issuer"];

        var securityKey = new SymmetricSecurityKey(Convert.FromHexString(jwtKey!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim("id", user.Id.ToString()),
            //new Claim("role", user.Role), // Optional if you have roles
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    // public string GenerateJwtTokenOrg(Organization org)
    // {
    //     var jwtKey = _configuration["Jwt:Key"];
    //     var jwtIssuer = _configuration["Jwt:Issuer"];

    //     var securityKey = new SymmetricSecurityKey(Convert.FromHexString(jwtKey!));
    //     var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    //     var claims = new[]
    //     {
    //         new Claim(JwtRegisteredClaimNames.Sub, org.Name),
    //         new Claim("id", org.Id.ToString()),
    //         //new Claim("role", user.Role), // Optional if you have roles
    //         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    //     };

    //     var token = new JwtSecurityToken(
    //         issuer: jwtIssuer,
    //         claims: claims,
    //         expires: DateTime.UtcNow.AddHours(2),
    //         signingCredentials: credentials
    //     );

    //     return new JwtSecurityTokenHandler().WriteToken(token);
    // }
}