
namespace RepairFirm.Shared.Models
{
    public class HomeData
    {
        public bool IsConnected { get; set; }
        public List<RepairCountChartData> RepairCountChartDatas { get; set; }
        public List<DepartmentContractData> DepartmentContractDatas { get; set; }
    }
}
