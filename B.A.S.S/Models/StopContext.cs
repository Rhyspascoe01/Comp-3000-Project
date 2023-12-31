﻿using Microsoft.EntityFrameworkCore;

namespace B.A.S.S.Models
{
    public class StopContext : DbContext
    {
        public DbSet<StopController> Stops { get; set; }
        public StopContext(DbContextOptions<StopContext> options) : base(options)
        {

        }
    }
}
