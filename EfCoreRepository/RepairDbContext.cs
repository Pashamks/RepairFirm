using EfCoreRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCoreRepository
{
    public class RepairDbContext : DbContext
    {
        private readonly string _connectionString = "Server=DESKTOP-SM098C1;Database=RepairFirma;TrustServerCertificate=True;Trusted_Connection=True;";
        public RepairDbContext()
        {

        }
        public RepairDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DbSet<Repair> Repair { get; set; }
        public DbSet<RepairType> RepairType { get; set; }
        public DbSet<RepairType> RepairTypes { get; set; }
        public DbSet<ExaminationPrice> ExaminationPrice { get; set; }
        public DbSet<ExaminationStatus> ExaminationStatus { get; set; }
        public DbSet<AreaExamination> AreaExamination { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Apartment> Apartment { get; set; }
        public DbSet<RoomType> RoomType { get; set; }
        public DbSet<RoomForExamination> RoomForExamination { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<RoomMaterial> RoomMaterial { get; set; }
        public DbSet<HistoryStatus> HistoryStatus { get; set; }
        public DbSet<RepairServices> RepairServices { get; set; }
        public DbSet<EmployeeForRepair> EmployeeForRepair { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<MaterialMeasure> MaterialMeasure { get; set; }
        public DbSet<MaterialType> MaterialType { get; set; }
        public DbSet<MaterialColor> MaterialColor { get; set; }
        public DbSet<MaterialProducer> MaterialProducer { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
