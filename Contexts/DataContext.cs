using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NeHauar.Models;

namespace NeHauar.Contexts
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("WebApiDatabase"));
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
    }
}