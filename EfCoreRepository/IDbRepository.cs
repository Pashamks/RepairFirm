

using RepairFirm.Shared.Models;

namespace EfCoreRepository
{
    public interface IDbRepository
    {
        public List<RepairServicesFactModel> GetRepairServicesFacts();
        public List<RepairCountChartData> GetRepairCountChart();
    }
}
