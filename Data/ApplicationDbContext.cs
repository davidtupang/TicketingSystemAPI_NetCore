using Microsoft.EntityFrameworkCore;
using TicketingSystem.Models;

namespace TicketingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Incident> Incidents { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
