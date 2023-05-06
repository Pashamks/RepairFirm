using Dapper;
using Microsoft.EntityFrameworkCore;
using RepairFirm.Shared.Models;
using System.Data;

namespace EfCoreRepository
{
    public class DbRepository : IDbRepository
    {
        private readonly IDbConnection _connection;
        public DbRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public List<DepartmentPaymentData> GetDepartmentPayments()
        {
            return _connection.Query<DepartmentPaymentData>("get_department_and_price", commandType: CommandType.StoredProcedure).ToList();
        }

        public List<DepartmentContractData> GetDepartmentServices()
        {
            return _connection.Query<DepartmentContractData>("get_department_and_count", commandType: CommandType.StoredProcedure).ToList();
        }

        public List<EmployeeForRepairData> GetEmployeeForRepairType()
        {
            return _connection.Query<EmployeeForRepairData>("get_employee_count_and_repair", commandType: CommandType.StoredProcedure).ToList();
        }

        public List<RepairCountChartData> GetRepairCountChart()
        {
            return _connection.Query<RepairCountChartData>("get_repair_name_and_count", commandType: CommandType.StoredProcedure).ToList();
        }

        public List<RepairServicesFactModel> GetRepairServicesFacts()
        {
            return _connection.Query<RepairServicesFactModel>("get_fact", commandType: CommandType.StoredProcedure).ToList();
        }

        public void PervLoading()
        {
            _connection.Query("delete_from_storage", commandType: CommandType.StoredProcedure).ToList();
            _connection.Query("delete_from_staging", commandType: CommandType.StoredProcedure).ToList();
            _connection.Query("load_data_into_staging_db ", commandType: CommandType.StoredProcedure).ToList();
            _connection.Query("tranform_and_upload_data", commandType: CommandType.StoredProcedure).ToList();
            _connection.Query("delete_from_staging", commandType: CommandType.StoredProcedure).ToList();
        }

        public void IncrementLoading()
        {
            _connection.Query("delete_from_staging", commandType: CommandType.StoredProcedure).ToList();
            _connection.Query("load_incremental_data_into_staging_db", commandType: CommandType.StoredProcedure).ToList();
            _connection.Query("upload_incremental", commandType: CommandType.StoredProcedure).ToList();
            _connection.Query("delete_from_staging", commandType: CommandType.StoredProcedure).ToList();
        }

        public List<RepairsByCitiesData> GetRepairsByCity()
        {
            return _connection.Query<RepairsByCitiesData>("get_repairs_by_cities", commandType: CommandType.StoredProcedure).ToList();
        }

        public MetadataModel GetMetaData()
        {
            var metadata = new MetadataModel();
            using(var ctx = new MetaDbContext())
            {
                var lastload = ctx.LoadHistory.OrderBy(x => x.LoadHistoryId).First();
                metadata.AvarageQueryTime = lastload.AvarageQueryTime;
                metadata.LastLoadDate = lastload.LastLoadDate;
                metadata.LoadDimentionsCount = lastload.LoadDimentionsCount;
                metadata.LoadAttributesCount = lastload.LoadAttributesCount;
                metadata.LoadValuesCount = lastload.LoadValusCount;
            }
            using(var ctx = new RepairDbContext())
            {
                metadata.OLTPTablesAndCount = new Dictionary<string, int>();
                var allTables = ctx.Model.GetEntityTypes();

                foreach (var table in allTables)
                {
                    Dictionary<string, int> OLTPTablesAndCount = new Dictionary<string, int>();

                    OLTPTablesAndCount.Add(nameof(ctx.Repair), ctx.Repair.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.RepairType), ctx.RepairType.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.RepairTypes), ctx.RepairTypes.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.ExaminationPrice), ctx.ExaminationPrice.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.ExaminationStatus), ctx.ExaminationStatus.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.AreaExamination), ctx.AreaExamination.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.Employee), ctx.Employee.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.Position), ctx.Position.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.Room), ctx.Room.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.Client), ctx.Client.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.Apartment), ctx.Apartment.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.RoomType), ctx.RoomType.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.RoomForExamination), ctx.RoomForExamination.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.Material), ctx.Material.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.RoomMaterial), ctx.RoomMaterial.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.HistoryStatus), ctx.HistoryStatus.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.RepairServices), ctx.RepairServices.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.EmployeeForRepair), ctx.EmployeeForRepair.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.Contract), ctx.Contract.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.Department), ctx.Department.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.MaterialMeasure), ctx.MaterialMeasure.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.MaterialType), ctx.MaterialType.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.MaterialColor), ctx.MaterialColor.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.MaterialProducer), ctx.MaterialProducer.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.Country), ctx.Country.Count());
                    OLTPTablesAndCount.Add(nameof(ctx.City), ctx.City.Count());

                    metadata.OLTPTablesAndCount = OLTPTablesAndCount;
                    metadata.OLTPDatabaseName = ctx.Database.GetDbConnection().Database;
                }
                metadata.TotalOLTPRows = metadata.OLTPTablesAndCount.Sum(x => x.Value);
                
            }
            using (var ctx = new StorageDbContext())
                metadata.StorageDatabaseName = ctx.Database.GetDbConnection().Database;
            return metadata;
        }

        public void ClearStorage()
        {
            _connection.Query("delete_from_storage", commandType: CommandType.StoredProcedure).ToList();
        }
    }
}
