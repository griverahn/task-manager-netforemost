using Microsoft.EntityFrameworkCore;
using NetForemost.Models;

namespace NetForemost.Data
{
    public class ApplicationDbContext : DbContext
    {
     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Priority> Priorities { get; set; }
    }
}
