using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.DTOs.Login;
using Domain.DTOs.Register;
using Domain.DTOs.Token;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class AuthService(
    UserManager<IdentityUser> userManager,
    IConfiguration config) : IAuthService
{
    public async Task<Response<TokenDto>> Login(LoginDto loginDto)
    {
        var user = await userManager.FindByNameAsync(loginDto.Username);
        if (user == null)
        {
            return new Response<TokenDto>(HttpStatusCode.BadRequest, "Username or password is incorrect");
        }

        var checkPassword = await userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!checkPassword)
        {
            return new Response<TokenDto>(HttpStatusCode.BadRequest, "Username or password is incorrect");
        }

        var token = await GenerateJwt(user);
        return new Response<TokenDto>(new TokenDto { Token = token });
    }

    public async Task<Response<string>> Register(RegisterDto registerDto)
    {
        var user = new IdentityUser
        {
            UserName = registerDto.Username,
            PhoneNumber = registerDto.Phone,
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        return !result.Succeeded
            ? new Response<string>(HttpStatusCode.InternalServerError, "Failed to create user")
            : new Response<string>("User created");
    }

    private async Task<string> GenerateJwt(IdentityUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber ?? "")
        };

        //claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
