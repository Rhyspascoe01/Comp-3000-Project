using Microsoft.EntityFrameworkCore;

namespace B.A.S.S.Models
{
    public class RouteContexts : DbContext
    {
        public DbSet<RouteController> RouteController { get; set; }
        public RouteContexts(DbContextOptions<RouteContexts> options) : base(options)
        {

        }
    }
}
