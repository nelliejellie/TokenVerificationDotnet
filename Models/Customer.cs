using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexRevision.Models
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
