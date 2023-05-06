

using RepairFirm.Shared.Models;

namespace EfCoreRepository
{
    public interface IDbRepository
    {
        List<RepairServicesFactModel> GetRepairServicesFacts();
        List<RepairCountChartData> GetRepairCountChart();
        List<DepartmentContractData> GetDepartmentServices();
        List<DepartmentPaymentData> GetDepartmentPayments();
        List<EmployeeForRepairData> GetEmployeeForRepairType();
        void PervLoading();
        void IncrementLoading();
        List<RepairsByCitiesData> GetRepairsByCity();
        MetadataModel GetMetaData();
    }
}
