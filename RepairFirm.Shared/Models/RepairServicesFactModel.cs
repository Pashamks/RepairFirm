
namespace RepairFirm.Shared.Models
{
    public class RepairServicesFactModel
    {
        public string IsRepairStartDate { get; set; }
        public string IsRepairEndDate { get; set; }
        public string IsRepairName { get; set; }
        public string IsContractId { get; set; }
        public string IsRepairCount { get; set; }
        public string IsRepairTotalHours { get; set; }
        public string IsRepairServiceTotalPrice { get; set; }
        public string IsRelationToTotalContractHours { get; set; }
        public string IRelationToTotalContractPrice { get; set; }
        public string IsEmployeeCount { get; set; }

        public DateTime RepairStartDate { get; set; }
        public DateTime RepairEndDate { get; set; }
        public string RepairName { get; set; }
        public int RoomId { get; set; }
        public int ContractId { get; set;}
        public int RepairCount { get; set; }
        public int RepairTotalHours { get; set; }
        public decimal RepairServiceTotalPrice { get; set; }
        public decimal RelationToTotalContractHours { get; set; }
        public decimal RelationToTotalContractPrice { get; set; }
        public int EmployeeCount { get; set; }

    }
}
