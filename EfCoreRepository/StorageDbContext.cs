using EfCoreRepository.Models;
using Microsoft.EntityFrameworkCore;


namespace EfCoreRepository
{
    public class StorageDbContext : DbContext
    {
        public DbSet<DimDate> DimDate { get; set; }
        public DbSet<DimCity> DimCity { get; set; }
        public DbSet<DimDepartment> DimDepartment { get; set; }
        public DbSet<DimClient> DimClient { get; set; }
        public DbSet<DimApartment> DimApartment { get; set; }
        public DbSet<DimRoom> DimRoom { get; set; }
        public DbSet<DimRepair> DimRepair { get; set; }
        public DbSet<DimContract> DimContract { get; set; }
        public DbSet<RepairServicesFact> RepairServicesFact { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-SM098C1;Database=RepairFirmaStorage;TrustServerCertificate=True;Trusted_Connection=True;");
        }
    }
}
