using SimplexRevision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexRevision.Contracts
{
    public interface IEmailService
    {
        Task<ResponseModel> SendEmail(EmailModel content, string body);
        Task<bool> SendToken(EmailModel content, string link);
    }
}
