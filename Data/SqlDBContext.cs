using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data
{
    public class SqlDBContext : DbContext
    {
        public SqlDBContext(DbContextOptions<SqlDBContext> opt) : base(opt) {}

        public DbSet<Command> Commands { get; set; }
    }
}