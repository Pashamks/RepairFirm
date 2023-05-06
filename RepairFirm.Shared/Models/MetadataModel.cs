
namespace RepairFirm.Shared.Models
{
    public class MetadataModel
    {
        public Dictionary<string, int> OLTPTablesAndCount { get; set; }
        public int TotalOLTPRows { get; set; } 
        public DateTime LastLoadDate { get; set; }
        public int LoadValuesCount { get; set; }
        public int LoadAttributesCount { get; set; }
        public int LoadDimentionsCount { get; set; }
        public decimal AvarageQueryTime { get; set; }
        public string OLTPDatabaseName { get; set; }
        public string StorageDatabaseName { get; set; }
    }
}
