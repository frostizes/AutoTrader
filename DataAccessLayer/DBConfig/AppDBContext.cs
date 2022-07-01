using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContractEntities.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DBConfig
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<TradeBot> TradeBot { get; set; }
        public DbSet<Crypto> Crypto { get; set; }
        public DbSet<Investment> Investments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "DataAccessLayer\\SqliteDatabase.db");
            options.UseSqlite($@"Data Source={path}");
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
