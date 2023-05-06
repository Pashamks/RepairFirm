using EfCoreRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCoreRepository
{
    public class MetaDbContext : DbContext
    {
        public DbSet<LoadHistory> LoadHistory { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-SM098C1;Database=RepairFirmaMeta;TrustServerCertificate=True;Trusted_Connection=True;");
        }
    }
}
