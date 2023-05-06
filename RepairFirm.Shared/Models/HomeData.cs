
namespace RepairFirm.Shared.Models
{
    public class HomeData
    {
        public bool IsConnected { get; set; }
        public List<RepairCountChartData> RepairCountChartDatas { get; set; }
        public List<DepartmentContractData> DepartmentContractDatas { get; set; }
        public List<EmployeeForRepairData> EmployeeForRepairDatas { get; set; }
        public Dictionary<string,List<RepairPriceForContract>> RepairsByCitiesDatas { get; set; }
    }
    public class RepairPriceForContract
    {
        public int TotalPrice { get; set; }
        public string Index { get; set; }
    }
}
