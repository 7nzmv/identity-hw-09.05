using Domain.DTOs.Login;
using Domain.DTOs.Register;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    DataContext context) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var user = new IdentityUser
        {
            UserName = registerDto.Username,
            PhoneNumber = registerDto.Phone,
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User registered");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var user = await userManager.FindByNameAsync(loginDto.Username);
        if (user == null)
            return BadRequest("Login or password is incorrect");

        var result =
            await signInManager.PasswordSignInAsync(user, loginDto.Password, true, false);

        if (!result.Succeeded)
            return BadRequest("Login or password is incorrect");

        return Ok("Logged in");
    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Ok("Logged out");
    }

    [HttpGet("students")]
    [Authorize]
    public async Task<IActionResult> GetStudents()
    {
        return Ok(await context.Students.ToListAsync());
    }
}

