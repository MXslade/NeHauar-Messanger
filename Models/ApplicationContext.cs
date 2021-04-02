using Microsoft.EntityFrameworkCore;

namespace NeHauar.Models {
    public class ApplicationContext : DbContext {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}

        public DbSet<Message> Messages { get; set; }
    }
}