using Microsoft.EntityFrameworkCore;
using SimplexRevision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplexRevision.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<TokenModel> Tokens { get; set; }
    }
}
