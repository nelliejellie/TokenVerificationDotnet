using SimplexRevision.Contracts;
using SimplexRevision.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexRevision.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _email;
        private readonly string _password;
        private readonly string _host;

        public EmailService(string email, string password, string host)
        {
            _email = email;
            _password = password;
            _host = host;
        }
        public async Task<ResponseModel> SendEmail(EmailModel content, string body)
        {
            content.Body = body;
            var mail = new MailMessage();
            mail.To.Add(new MailAddress(content.To));
            mail.From = new MailAddress(_email);
            mail.Subject = content.Subject;
            mail.Body = content.Body;
            mail.IsBodyHtml = true;

            try
            {
                using (var smtp = new SmtpClient())
                {
                    var credentials = new NetworkCredential(_email, _password, _host);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credentials;
                    smtp.Host = _host;
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    

                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception Ex)
            {
                return new ResponseModel { Message = Ex.Message, Success = false };
            }

            return new ResponseModel { Message = "Successful", Success = true };
        }

        public Task<bool> SendToken(EmailModel content, string link)
        {
            throw new NotImplementedException();
        }
    }
}
