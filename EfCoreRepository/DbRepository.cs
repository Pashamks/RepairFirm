using Dapper;
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

        public List<RepairCountChartData> GetRepairCountChart()
        {
            return _connection.Query<RepairCountChartData>("get_repair_name_and_count", commandType: CommandType.StoredProcedure).ToList();
        }

        public List<RepairServicesFactModel> GetRepairServicesFacts()
        {
            return _connection.Query<RepairServicesFactModel>("get_fact", commandType: CommandType.StoredProcedure).ToList();
        }
    }
}
