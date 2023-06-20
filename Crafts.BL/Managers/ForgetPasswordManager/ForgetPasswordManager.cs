using Crafts.BL.Dtos.IdentityDtos;
using Crafts.DAL.Models;
using Crafts.DAL.Repos.IdentityRepo;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.ForgetPasswordManager
{
    public class ForgetPasswordManager : IForgetPasswordManager
    {
        private readonly IUserRepo _userRepo;

        public ForgetPasswordManager(IUserRepo userRepo)
        {
            
            _userRepo = userRepo;
        }

        #region VerifyPassword

        
        public void VerifyPassword(HashcodeDto codedto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to a byte array
                byte[] inputBytes = Encoding.UTF8.GetBytes(codedto.code.ToString());

                // Compute the hash value of the input bytes
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the hash bytes to a string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                User? user =  _userRepo.GetUserByHashCode(sb.ToString());


                if (user is null)
                {
                    throw new ArgumentException("Invalid Code");
                }
                if (user?.ExpirationDate < DateTime.Now)
                {
                    throw new ArgumentException("Date is expired");

                }
                Console.WriteLine(user);
                user.Flag= true;
                _userRepo.Update(user);
                _userRepo.SaveChanges();

            }

        }
        #endregion
    }
}
