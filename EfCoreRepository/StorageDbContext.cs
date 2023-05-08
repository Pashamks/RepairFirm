using EfCoreRepository.Models;
using Microsoft.EntityFrameworkCore;


namespace EfCoreRepository
{
    public class StorageDbContext : DbContext
    {
        private readonly string _connectionString = "";
        public StorageDbContext()
        {

        }
        public StorageDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string sql = @"
            CREATE PROCEDURE get_fact 
            AS 
            BEGIN 

            SELECT 
                   DATEFROMPARTS(dds.[year], dds.[month], dds.[day]) as RepairStartDate
                  ,DATEFROMPARTS(dde.[year], dde.[month], dde.[day]) as RepairEndDate
                  ,dr.repairName 
                  ,[roomId]
                  ,[contractId]
                  ,[repairCount]
                  ,[repairTotalHours]
                  ,[repairServiceTotalPrice]
                  ,[relationToTotalContractHours]
                  ,[relationToTotalContractPrice]
                  ,[employeeCount]
              FROM [RepairFirmaStorage].[dbo].[RepairServicesFact] as rsf
              INNER JOIN [RepairFirmaStorage].[dbo].DimDate as dde on dde.dateKey = rsf.repairEndDateId
              INNER JOIN [RepairFirmaStorage].[dbo].DimDate as dds on dds.dateKey = rsf.repairStartDateId
              INNER JOIN [RepairFirmaStorage].[dbo].DimRepair as dr on dr.repairKey = rsf.repairId

              END 
            GO


            CREATE PROCEDURE get_repair_name_and_count
            AS 
            BEGIN 

            SELECT 
                   dr.repairType,
	              COUNT(dr.repairId) as repairCount
              FROM [RepairFirmaStorage].[dbo].[RepairServicesFact] as rsf
              INNER JOIN [RepairFirmaStorage].[dbo].DimRepair as dr on dr.repairKey = rsf.repairId
              GROUP BY dr.repairType
              END 
            GO

            CREATE PROCEDURE get_department_and_count
            AS 
            BEGIN 

            SELECT 
                   dci.cityName,
	              COUNT(dc.contractId) as servicesCount
              FROM [RepairFirmaStorage].[dbo].DimContract as dc
              INNER JOIN [RepairFirmaStorage].[dbo].DimDepartment as dd on dd.departmentId = dc.departmentId
              INNER JOIN [RepairFirmaStorage].[dbo].DimCity as dci on dci.cityKey = dd.cityId
              GROUP BY dci.cityName
              END 
            GO

            CREATE PROCEDURE get_department_and_price
            AS 
            BEGIN 

            SELECT dc.totalPrice, t.cityName FROM [RepairFirmaStorage].[dbo].DimContract as dc
             INNER JOIN [RepairFirmaStorage].[dbo].DimDepartment as dd on dd.departmentId = dc.departmentId
              INNER JOIN [RepairFirmaStorage].[dbo].DimCity as dci on dci.cityKey = dd.cityId
              INNER JOIN 
            (SELECT TOP 3
                   dci.cityName
	               FROM [RepairFirmaStorage].[dbo].DimContract as dc
              INNER JOIN [RepairFirmaStorage].[dbo].DimDepartment as dd on dd.departmentId = dc.departmentId
              INNER JOIN [RepairFirmaStorage].[dbo].DimCity as dci on dci.cityKey = dd.cityId
              GROUP BY dci.cityName
              ORDER BY COUNT(dc.contractId) DESC) as t
              ON t.cityName = dci.cityName
              END 
            GO

            CREATE PROCEDURE get_employee_count_and_repair
            AS 
            BEGIN 

            SELECT dr.repairType, SUM(rsf.employeeCount) as empoyeeCount FROM [RepairFirmaStorage].[dbo].RepairServicesFact as rsf
            INNER JOIN [RepairFirmaStorage].[dbo].DimRepair as dr  on rsf.repairId = dr.repairId
            GROUP BY dr.repairType
	
            END 
            GO

            CREATE PROCEDURE get_repairs_by_cities
                        AS 
                        BEGIN 
            SELECT  dr.repairType, rsf.repairCount, dci.cityName
              FROM [RepairFirmaStorage].[dbo].[RepairServicesFact] as rsf
              INNER JOIN [RepairFirmaStorage].[dbo].DimContract as dc on dc.contractKey = rsf.contractId
              INNER JOIN [RepairFirmaStorage].[dbo].DimDepartment as dd on dd.departmentKey = dc.departmentId
              INNER JOIN [RepairFirmaStorage].[dbo].DimRepair as dr on dr.repairKey = rsf.repairId
              INNER JOIN [RepairFirmaStorage].[dbo].DimCity as dci on dci.cityKey = dd.cityId
              END 
             GO
            ";

        }
    }
}
