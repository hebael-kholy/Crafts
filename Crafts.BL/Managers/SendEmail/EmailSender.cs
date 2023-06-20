using Crafts.BL.Dtos.IdentityDtos;
using Crafts.DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.SendEmail
{
    public class EmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //UserReadDto user1;

        public async Task SendEmail(User user)
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);

            var apiKey = _configuration.GetValue<string>("MailApiKey");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("Jamal.Ali.Habashi@gmail.com", "Crafts❤️");
            var subject = "Your reset password code (valid for 10 minutes)";
            var to = new EmailAddress(user.Email, user.UserName);
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = $"<strong>Hello, {user.UserName} </strong><br>" +
                $"We recieved a request to rest the password on Crafts Account<br>"+
                $"<strong>{randomNumber}</strong> Enter this code to completethe reset"+
                "Thanks for helping us keep your account secure.<br>" +
                "The Craft Team";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to a byte array
                byte[] inputBytes = Encoding.UTF8.GetBytes(randomNumber.ToString());

                // Compute the hash value of the input bytes
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the hash bytes to a string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                 
                user.HashCode = sb.ToString();
                user.ExpirationDate = DateTime.Now.AddMinutes(10);
                user.Flag= false;
            }
        }
    }
}
