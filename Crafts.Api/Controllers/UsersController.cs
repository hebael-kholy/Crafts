using Crafts.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Crafts.BL.Dtos.IdentityDtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.OpenApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Crafts.DAL.Models.Enum;

namespace Crafts.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
     private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        public UsersController(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

    #region Register
    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new User
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
            Gender = registerDto.Gender,
            Role = Role.User
        };
        var userCreationresult = await _userManager.CreateAsync(user, registerDto.Password);
        if (!userCreationresult.Succeeded)
        {
            return BadRequest(userCreationresult.Errors);
        }

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, Role.User.ToString()),

            };

        await _userManager.AddClaimsAsync(user, claims);

        return Ok(user);
    }
    #endregion

   
    #region Login
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<TokenDto>> Login(LoginDto cradentials)
    {
        User? user = await _userManager.FindByEmailAsync(cradentials.Email);
        if (user is null)
        {
            return BadRequest(new { Message = "User Not Found" });
        }
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, cradentials.Password);
        if (!isPasswordCorrect)
        {
            return Unauthorized();
        }
        var claims = await _userManager.GetClaimsAsync(user);
        DateTime exp = DateTime.Now.AddMinutes(20);

        var token = GenerateToken(claims, exp);


        return  new TokenDto(token) ;
        
    }
    #endregion   
    #region Login
    [HttpPost]
    [Route("LoginAdmin")]
    [Authorize(Policy = "AllowAdminsOnly")]
    public async Task<ActionResult<TokenDto>> LoginAdmin(LoginDto cradentials)
    {
        User? user = await _userManager.FindByEmailAsync(cradentials.Email);
        if (user is null)
        {
            return BadRequest(new { Message = "User Not Found" });
        }
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, cradentials.Password);
        if (!isPasswordCorrect || Role.Admin == 0)
        {
            return Unauthorized();
        }    

        var claims = await _userManager.GetClaimsAsync(user);
        DateTime exp = DateTime.Now.AddMinutes(20);

        var token = GenerateToken(claims, exp);


        return  new TokenDto(token) ;
        
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
