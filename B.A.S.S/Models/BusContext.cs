using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B.A.S.S.Models
{
    public class BusContext : DbContext
    {
        public DbSet<BusController> BusController { get; set; }
        public BusContext(DbContextOptions<BusContext> options) : base(options)
        {

        }
    }
}
