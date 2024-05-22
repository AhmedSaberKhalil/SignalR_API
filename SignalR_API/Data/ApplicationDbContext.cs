using Microsoft.EntityFrameworkCore;
using SignalR_API.Models;

namespace SignalR_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public ApplicationDbContext()
        {
            
        }

        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }


    }
}
