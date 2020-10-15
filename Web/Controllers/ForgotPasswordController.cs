using Microsoft.AspNetCore.Mvc;
using Platform_Racing_3_Common.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace Platform_Racing_3_Web.Controllers
{
    [ApiController]
    [Route("forgotpassword")]
    public class ForgotPasswordController : ControllerBase
    {
        [HttpPost]
        public async Task<string> PostAsync([FromForm] string email)
        {
            PlayerUserData player = await UserManager.TryGetUserDataByEmailAsync(email);
            if (player != null)
            {
                if (!await UserManager.CanSendPasswordToken(player.Id))
                {
                    return "Please request new password reset token after half an hour!";
                }

                //Generate 512 bit random token
                byte[] token = new byte[512 / 8];

                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetNonZeroBytes(token.AsSpan(start: 0, length: 32));
                }

                using (SHA256 sha = SHA256.Create())
                {
                    byte[] content = Encoding.UTF8.GetBytes(player.Id.ToString() + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + email);

                    if (!sha.TryComputeHash(content, token.AsSpan(start: 32, length: 32), out int bytesWritten) || bytesWritten != 32)
                    {
                        //Should not happen, right?

                        return "Something went wrong when trying to generate the token! Try again!";
                    }
                }

                byte[] tokenHash;

                //Generate hash of the token for database
                using (SHA512 sha = SHA512.Create())
                {
                    tokenHash = sha.ComputeHash(token);
                }

                bool result = await UserManager.TryCreateForgotPasswordToken(player.Id, Convert.ToBase64String(tokenHash), this.HttpContext.Connection.RemoteIpAddress);
                if (!result)
                {
                    return "Something went wrong when trying to create the token! Try again!";
                }

                using (MailMessage mail = new MailMessage())
                {
                    mail.To.Add(email);
                    mail.Subject = $"Platform Racing 3 - {player.Username} - Password Reset";
                    mail.From = new MailAddress(Program.Config.SmtpUser);
                    mail.Body = $"You have requested password reset for your {player.Username} account! Use the following link to reset your password! https://pr3hub.com/resetpassword?email={email}&token={WebEncoders.Base64UrlEncode(token)}";

                    Program.SmtpClient.Send(mail);
                }

                return "We have sent you email!";
            }
            else
            {
                return "User was not found!";
            }
        }
    }
}
