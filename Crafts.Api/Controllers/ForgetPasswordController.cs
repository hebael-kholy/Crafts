using Crafts.BL.Dtos.IdentityDtos;
using Crafts.BL.Managers.ForgetPasswordManager;
using Crafts.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Crafts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForgetPasswordController : ControllerBase
    {
        private readonly IForgetPasswordManager _forgetPasswordManager;

        public ForgetPasswordController(IForgetPasswordManager forgetPasswordManager)
        {
            
            _forgetPasswordManager = forgetPasswordManager;
        }
        #region VerifyPassword

        [HttpPost]
        [Route("VerifyPassword")]
        public  ActionResult VerifyPassword(HashcodeDto codedto)
        {
            
            _forgetPasswordManager.VerifyPassword(codedto);
            return Ok();

        }
        #endregion
    }
}
