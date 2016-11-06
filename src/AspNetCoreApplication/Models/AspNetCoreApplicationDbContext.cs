using Microsoft.EntityFrameworkCore;

namespace AspNetCoreApplication.Models
{
    public class AspNetCoreApplicationDbContext : DbContext
    {
        public AspNetCoreApplicationDbContext(DbContextOptions<AspNetCoreApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TodoModel> Todos { get; set; }
    }
}
