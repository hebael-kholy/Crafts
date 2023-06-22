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
using System.Runtime.Intrinsics.X86;
using System.IO;
using System.Net.Mail;
using System.Net;
using SendGrid.Helpers.Mail;
using SendGrid;
using Crafts.BL.Managers.SendEmail;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Security.Cryptography;

namespace Crafts.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly IWebHostEnvironment _env;
    private readonly EmailSender _emailSender;

    //private readonly RoleManager<User> _roleManager;

    public UsersController(IConfiguration configuration,
        UserManager<User> userManager,
        IWebHostEnvironment env,
        EmailSender emailSender)
    {
        _configuration = configuration;
        _userManager = userManager;
        _env = env;
        _emailSender = emailSender;
        //_roleManager = roleManager;
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
            Role = registerDto.Role
        };
        var userCreationresult = await _userManager.CreateAsync(user, registerDto.Password);
        if (!userCreationresult.Succeeded)
        {
            return BadRequest(userCreationresult.Errors);
        }
        var UserRoles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, user.Role.ToString()),

            };
        DateTime exp = DateTime.Now.AddMinutes(20);

        var token = GenerateToken(claims, exp);
        await _userManager.AddClaimsAsync(user, claims);
        var res = new { user, token };
        return Ok(res);
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
        var UserRole = await _userManager.GetRolesAsync(user);
        if (!isPasswordCorrect)
        {
            return Unauthorized();
        }
        var claims = await _userManager.GetClaimsAsync(user);
        DateTime exp = DateTime.Now.AddMinutes(20);

        var token = GenerateToken(claims, exp);

        var res = new { user,token };
        return Ok(res);

    }
    #endregion   

    #region LoginAdmin
    [HttpPost]
    [Route("LoginAdmin")]
    //[Authorize(Policy = "AllowAdminsOnly")]
    //[Authorize(Roles = "1")]
    public async Task<ActionResult<TokenDto>> LoginAdmin(LoginDto cradentials)
    {
        User? user = await _userManager.FindByEmailAsync(cradentials.Email);
        if (user is null)
        {
            return BadRequest(new { Message = "User Not Found" });
        }
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, cradentials.Password);
        if (!(isPasswordCorrect && user.Role == 0))
        {
            return Unauthorized();
        }

        var claims = await _userManager.GetClaimsAsync(user);
        DateTime exp = DateTime.Now.AddMinutes(20);

        var token = GenerateToken(claims, exp);


        return new TokenDto(token);

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

    #region GetAllUsers
    [HttpGet]
    //[Authorize(Policy = "AllowAdminsOnly")]
    public ActionResult<UserReadDto> GetAll()
    {
        var userData = _userManager.Users.Where(R => R.Role == 0);

        return Ok(userData);
    }
    #endregion

    #region GetUserById
    [HttpGet]
    [Route("{id}")]
    //[Authorize(Policy = "AllowAdminsOnly")]
    public async Task<ActionResult<UserReadDto>> GetById(string id)
    {
        var User = await _userManager.FindByIdAsync(id);

        var UserData = new
        {
            Email = User.Email,
            UserName = User.UserName,
            PasswordHash = User.PasswordHash,
            Image = User.Image,
            Gender = User.Gender.GetDisplayName(),
        };
        return Ok(UserData);


    }
    #endregion

    #region deleteUserById
    [HttpDelete]
    [Route("{id}")]
    //[Authorize(Policy = "AllowAdminsOnly")]
    public async Task<ActionResult<bool>> deleteById(string id)
    {


        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound($"User with ID {user} not found.");
        }

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;

    }
    #endregion


    #region UpdateUser

    [HttpPut("update/{userId}")]
    public async Task<ActionResult<UpdateUserDto>> UpdateUser(string userId, [FromBody] UpdateUserDto updateDto)
    {

        // Step 2: Validate the input data
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Step 3: Retrieve the user object
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        // Step 5: Update the other fields of the user object
        user.UserName = updateDto.UserName ?? user.UserName;
        user.Email = updateDto.Email ?? user.Email;

        var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, updateDto.Password ?? user.PasswordHash);
        user.PasswordHash = newPasswordHash;

        // Step 6: Save the updated user object
        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded)
        {
            return BadRequest(res.Errors);
        }
        var result = new { res.Succeeded, user};
        return Ok(result);
    }

    #endregion

    #region ForgetPassword

    [HttpPost]
    [Route("Reset-Password")]
    public async Task<IActionResult> SendEmail(ForgetPasswordDto passwordDto)
    {

        User? user = await _userManager.FindByEmailAsync(passwordDto.Email);

        if (user is null)
        {
            return BadRequest(new { Message = "User Not Found" });
        }

        
        _emailSender.SendEmail(user).Wait();
        await _userManager.UpdateAsync(user);
        return Ok();
    }

    #endregion

    #region ResetPassword

    [HttpPut]
    [Route("NewPassword")]
    public async Task<ActionResult> ResetPassword(ResetPasswordDto resetDto)
    {

        User? user = await _userManager.FindByEmailAsync(resetDto.Email);

        if (user is null)
        {
            return BadRequest(new { Message = "User Not Found" });
        }
        if (user.Flag == false)
        {
            return BadRequest(new { Message = "Reset Code Is not Verified" });

        }
        var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, resetDto.Password);
        user.PasswordHash = newPasswordHash;
        user.Flag = false;
        user.ExpirationDate = null;
        user.HashCode = null;
        // Step 6: Save the updated user object
        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded)
        {
            return BadRequest(res.Errors);
        }
        return Ok("Reset Successfully");
    }
    #endregion

    #region UpdateImage
    private bool IsSupportedImageFormat(IFormFile file)
    {
        return file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/jpg" || file.ContentType == "image/webp";
    }

    [HttpPut("image/{userId}")]
    public async Task<ActionResult<UserImageDto>> UpdateUserImage(string userId, [FromForm] UserImageDto userImageDto)
    {

        // Step 2: Validate the input data
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (userImageDto.image != null && !IsSupportedImageFormat(userImageDto.image))
        {
            return BadRequest("Image file must be in JPEG or PNG or JPG or WEBP format.");
        }

        // Step 3: Retrieve the user object
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"User with ID {userId} not found.");
        }

        // Step 4: Save the new image file
        var imgURL = upload.UploadImageOnCloudinary(userImageDto.image);

        user.Image = imgURL;

        // Step 6: Save the updated user object
        var res = await _userManager.UpdateAsync(user);
        if (!res.Succeeded)
        {
            return BadRequest(res.Errors);
        }
        var result = new { res.Succeeded, user.Image };
        return Ok(result);
    }
    #endregion
}