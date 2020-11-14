using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> opt) : base(opt) {}

        public DbSet<Command> Commands { get; set; }
    }
}