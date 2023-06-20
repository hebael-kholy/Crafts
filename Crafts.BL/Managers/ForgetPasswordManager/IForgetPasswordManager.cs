using Crafts.BL.Dtos.IdentityDtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.ForgetPasswordManager
{
    public interface IForgetPasswordManager
    {
        public void VerifyPassword(HashcodeDto codedto);

    }
}
