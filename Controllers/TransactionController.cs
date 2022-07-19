using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimplexRevision.Contracts;
using SimplexRevision.Data;
using SimplexRevision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SimplexRevision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IEmailService _email;
        private readonly AppDbContext _database;

        public TransactionController(IEmailService email, AppDbContext database)
        {
            _email = email;
            _database = database;
        }

        [HttpPost]
        [Route("sendToken")]
        public async Task<IActionResult> SendToken(string email)
        {
            // create a token
            var token = new Random().Next(0,10000).ToString("D5");

            // hash it
            SHA512 hasher = SHA512.Create();

            var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(token));

            var hex = BitConverter.ToString(hash);

            var tokenHolder = new TokenModel
            {
                Token = hex
            };

            await _database.Tokens.AddAsync(tokenHolder);

            await _database.SaveChangesAsync();

            var newEmail = new EmailModel
            {
                To = email,
                Body = token,
                Subject = "you token for accessing the payment service"
            };
            var IsSuccessful = await _email.SendEmail(newEmail, token);

            if (IsSuccessful.Success)
            {
                return Ok("your token has been sent to your email");
            }

            return BadRequest(IsSuccessful.Message);
        }

        [HttpPost]
        [Route("verifyToken")]
        public async Task<IActionResult> VerifyToken(string token)
        {
            SHA512 hasher = SHA512.Create();

            var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(token));

            var hex = BitConverter.ToString(hash);

            var storedToken = await _database.Tokens.FirstOrDefaultAsync(t => t.Token.Equals(hex));

            if(storedToken == null)
            {
                return BadRequest("token does not exist");
            }

            return Ok("you can safely access your transaction");
        }
    }
}
