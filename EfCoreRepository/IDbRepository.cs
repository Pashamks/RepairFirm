

using RepairFirm.Shared.Models;

namespace EfCoreRepository
{
    public interface IDbRepository
    {
        public List<RepairServicesFactModel> GetRepairServicesFacts();
        public List<RepairCountChartData> GetRepairCountChart();
        public List<DepartmentContractData> GetDepartmentServices();
        public List<DepartmentPaymentData> GetDepartmentPayments();
        public List<EmployeeForRepairData> GetEmployeeForRepairType();
    }
}
