using Microsoft.EntityFrameworkCore;
using ShowPass.Models;

namespace ShowPass.Data
{
    public class ShowPassDbContext : DbContext
    {
        public ShowPassDbContext(DbContextOptions<ShowPassDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}