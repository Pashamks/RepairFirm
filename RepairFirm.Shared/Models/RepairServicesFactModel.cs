
namespace RepairFirm.Shared.Models
{
    public class RepairServicesFactModel
    {
        public DateTime RepairStartDate { get; set; }
        public DateTime RepairEndDate { get; set; }
        public string RepairName { get; set; }
        public int RoomId { get; set; }
        public int ContractId { get; set;}
        public int RepairCount { get; set; }
        public int RepairTotalHours { get; set; }
        public int RepairTotalPrcie { get; set; }
        public decimal RelationToTotalContractHours { get; set; }
        public decimal RelationToTotalContractPrice { get; set; }
        public int EmployeeCount { get; set; }

    }
}
