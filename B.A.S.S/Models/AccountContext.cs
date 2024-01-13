using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B.A.S.S.Models
{
    public class AccountContext : DbContext
    {
        public DbSet<AccountController> AccountControllers { get; set; }
        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {

        }
    }
}