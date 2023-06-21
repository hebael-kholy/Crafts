using Crafts.BL.Dtos.IdentityDtos;
using Crafts.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Crafts.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _env;

    //private readonly RoleManager<User> _roleManager;

    public AdminsController(IConfiguration configuration,
        UserManager<User> userManager,
        IWebHostEnvironment env)
    {
        _configuration = configuration;
        _userManager = userManager;
        _env = env;
        //_roleManager = roleManager;
    }


    #region LoginAdmin
    [HttpPost]
[Route("LoginAdmin")]
public async Task<ActionResult<TokenDto>> LoginAdmin(LoginDto cradentials)
{
    User? user = await _userManager.FindByEmailAsync(cradentials.Email);
    if (user is null)
    {
        return BadRequest(new { Message = "User Not Found" });
    }
    var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, cradentials.Password);
    if (!isPasswordCorrect && user.Role==0)
    {
        return Unauthorized();
    }

    var claims = await _userManager.GetClaimsAsync(user);
    DateTime exp = DateTime.Now.AddMinutes(20);

    var token = GenerateToken(claims, exp);

    var res = new { user, token };
    return Ok(res);

}
    #endregion

    #region GenerateToken
    private string GenerateToken(IList<Claim> claimsList, DateTime exp)
    {
        var SecretKeyString = _configuration.GetValue<string>("SecretKey");
        var SecretKeyInBytes = Encoding.ASCII.GetBytes(SecretKeyString);
        var SecurityKey = new SymmetricSecurityKey(SecretKeyInBytes);

        var signingCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            claims: claimsList,
            expires: exp,
            signingCredentials: signingCredentials);
        // convert token to string
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(jwt);
        return tokenString;
    }
    #endregion
}
