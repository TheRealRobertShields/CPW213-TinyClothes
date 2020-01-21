using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyClothes.Models;

namespace TinyClothes.Data
{
    public class StoreContext : DbContext 
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }

        // Add a DbSet for each Entity that needs to be tracked by the DB
        public DbSet<Clothing> Clothing { get; set; }
    }
}
