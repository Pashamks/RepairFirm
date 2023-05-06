
namespace RepairFirm.Shared.Models
{
    public class FilterRepairServicesFactModel
    {
        public string? IsMinRepairStartDate { get; set; }
        public string? IsMaxRepairStartDate { get; set; }
        public string? IsMinRepairEndDate { get; set; }
        public string? IsMaxRepairEndDate { get; set; }
        public string? IsRepairName { get; set; }
        public string? IsContractId { get; set; }
        public string? IsRoomId { get; set; }
        public string? IsMinRepairCount { get; set; }
        public string? IsMaxRepairCount { get; set; }
        public string? IsMinRepairTotalHours { get; set; }
        public string? IsMaxRepairTotalHours { get; set; }
        public string? IsMinRepairServiceTotalPrice { get; set; }
        public string? IsMaxRepairServiceTotalPrice { get; set; }
        public string? IsMinRelationToTotalContractHours { get; set; }
        public string? IsMaxRelationToTotalContractHours { get; set; }
        public string? IMinRelationToTotalContractPrice { get; set; }
        public string? IMaxRelationToTotalContractPrice { get; set; }
        public string? IsMinEmployeeCount { get; set; }
        public string? IsMaxEmployeeCount { get; set; }

        public string? MinRepairStartDate { get; set; }
        public DateTime MaxRepairStartDate { get; set; }
        public DateTime MinRepairEndDate { get; set; }
        public DateTime MaxRepairEndDate { get; set; }
        public string RepairName { get; set; }
        public int RoomId { get; set; }
        public int ContractId { get; set; }
        public int MinRepairCount { get; set; }
        public int MaxRepairCount { get; set; }
        public int MinRepairTotalHours { get; set; }
        public int MaxRepairTotalHours { get; set; }
        public decimal MinRepairServiceTotalPrice { get; set; }
        public decimal MaxRepairServiceTotalPrice { get; set; }
        public decimal MinRelationToTotalContractHours { get; set; }
        public decimal MaxRelationToTotalContractHours { get; set; }
        public decimal MinRelationToTotalContractPrice { get; set; }
        public decimal MaxRelationToTotalContractPrice { get; set; }
        public int MinEmployeeCount { get; set; }
        public int MaxEmployeeCount { get; set; }
        public string? IsMinRelationToTotalContractPrice { get; set; }
        public string? IsMaxRelationToTotalContractPrice { get; set; }
    }
}
