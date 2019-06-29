using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Platform_Racing_3_Common.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Platform_Racing_3_Web.Controllers
{
    [Route("resetpassword")]
    public class ResetPasswordController : Controller
    {
        [HttpPost]
        public async Task<string> PostAsync([FromForm] string email, [FromForm] string token, [FromForm] string password)
        {
            //Password check
            if (string.IsNullOrWhiteSpace(password))
            {
                return "Please enter password";
            }
            else if (password.Length < RegisterController.PASSWORD_MIN_LENGTH)
            {
                return $"Password must be at least {RegisterController.PASSWORD_MIN_LENGTH} chars long";
            }

            PlayerUserData player = await UserManager.TryGetUserDataByEmailAsync(email);
            if (player == null)
            {
                return "Invalid token";
            }

            string hashedToken;
            using (SHA512 sha = SHA512.Create())
            {
                hashedToken = Convert.ToBase64String(sha.ComputeHash(Base64UrlEncoder.DecodeBytes(token)));
            }

            uint userId = await UserManager.TryGetForgotPasswordToken(hashedToken);
            if (userId == 0 || userId != player.Id)
            {
                return "Invalid token";
            }

            if (!await UserManager.TryConsumePasswordToken(hashedToken, password, this.HttpContext.Connection.RemoteIpAddress))
            {
                return "Error, try again";
            }

            using (MailMessage mail = new MailMessage())
            {
                mail.To.Add(email);
                mail.Subject = $"Platform Racing 3 - {player.Username} - Security Alert";
                mail.From = new MailAddress(Program.Config.SmtpUser);
                mail.Body = $"Your account {player.Username} password was reset! If you did not do this change your password immediately!";

                Program.SmtpClient.Send(mail);
            }

            return "Password has been reset! Start the grind :sunglasses:";
        }
    }
}
