using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.DTOs.Register;
using Domain.DTOs.Login;

namespace IdentityWithCookie.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    DataContext context) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var user = new IdentityUser
        {
            UserName = model.Username,
            PhoneNumber = model.Phone,
        };

        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User registered");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await userManager.FindByNameAsync(model.Username);
        if (user == null)
            return BadRequest("Login or password is incorrect");

        var result =
            await signInManager.PasswordSignInAsync(user, model.Password, true, false);

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

    [HttpGet("teachers")]
    [Authorize]
    public async Task<IActionResult> GetStudents()
    {
        return Ok(await context.Students.ToListAsync());
    }
}