
namespace EfCoreRepository.Models
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class Position
    {
        public int PositionId { get; set; }
        public string position { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class Country
    {
        public int CountryId { get; set; }
        public string country { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class Client
    {
        public int ClientId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class RepairType
    {
        public int RepairTypeId { get; set; }
        public string repairType { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class MaterialColor
    {
        public int MaterialColorId { get; set; }
        public string materialColor { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class RoomType
    {
        public int RoomTypeId { get; set; }
        public string roomType { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class ExaminationPrice
    {
        public int ExaminationPriceId { get; set; }
        public decimal MinSquare { get; set; }
        public decimal MaxSquare { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class MaterialType
    {
        public int MaterialTypeId { get; set; }
        public string materialType { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class MaterialMeasure
    {
        public int MaterialMeasureId { get; set; }
        public string materialMeasure { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class Department
    {
        public int DepartmentId { get; set; }
        public int CityId { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual City City { get; set; }
    }
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public int PositionId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Position Position { get; set; }
    }
    public class Apartment
    {
        public int ApartmentId { get; set; }
        public int ClientId { get; set; }
        public int CityId { get; set; }
        public int RoomsCount { get; set; }
        public string Address { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class MaterialProducer
    {
        public int MaterialProducerId { get; set; }
        public string materialProducer { get; set; }
        public int CountryId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual Country Country { get; set; }
    }
    public class ExaminationStatus
    {
        public int ExaminationStatusId { get; set; }
        public string examinationStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class HistoryStatus
    {
        public int HistoryStatusId { get; set; }
        public string historyStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class Room
    {
        public int RoomId { get; set; }
        public int ApartmentId { get; set; }
        public int RoomTypeId { get; set; }
        public decimal FloorSquare { get; set; }
        public decimal WallsSquare { get; set; }
        public decimal CellingSquare { get; set; }
        public int DoorsCount { get; set; }
        public int WindowsCount { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class Material
    {
        public int MaterialId { get; set; }
        public string MaterialName { get; set; }
        public int MaterialProducerId { get; set; }
        public int MaterialColorId { get; set; }
        public int MaterialTypeId { get; set; }
        public int MaterialMeasureId { get; set; }
        public int MaterialVolume { get; set; }
        public decimal MaterialPrice { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class Repair
    {
        public int RepairId { get; set; }
        public int RepairTypeId { get; set; }
        public string RepairName { get; set; }
        public string RepairDescription { get; set; }
        public decimal RepairMinHours { get; set; }
        public decimal RepairMaxHours { get; set; }
        public decimal RepairPrice { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class AreaExamination
    {
        public int AreaExaminationId { get; set; }
        public int EmployeeId { get; set; }
        public int ExaminationPriceId { get; set; }
        public int ExaminationStatusId { get; set; }
        public string Comments { get; set; }
        public decimal WorkEstimatedPrice { get; set; }
        public decimal MaterialsEstimatedPrice { get; set; }
        public DateTime ExaminationDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class RoomForExamination
    {
        public int RoomForExaminationId { get; set; }
        public int AreaExaminationId { get; set; }
        public int RoomId { get; set; }
        public int? RepairServicesId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class RepairServices
    {
        public int RepairServicesId { get; set; }
        public int? RoomForExaminationId { get; set; }
        public int HistoryStatusId { get; set; }
        public int RepairId { get; set; }
        public int RepairCount { get; set; }
        public DateTime RepairStartDate { get; set; }
        public DateTime? RepairEndDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class RoomMaterial
    {
        public int RoomMaterialId { get; set; }
        public int RoomForExaminationId { get; set; }
        public int MaterialId { get; set; }
        public int MaterialCount { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class EmployeeForRepair
    {
        public int EmployeeForRepairId { get; set; }
        public int EmployeeId { get; set; }
        public int RepairServicesId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }


    public class Contract
    {
        public int ContractId { get; set; }
        public int AreaExaminationId { get; set; }
        public int HistoryStatusId { get; set; }
        public int DepartmentId { get; set; }
        public decimal Prepayment { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    /*
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */

    public class DimDate
    {
        public int DateKey { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class DimCity
    {
        public int CityKey { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
    }

    public class DimDepartment
    {
        public int DepartmentKey { get; set; }
        public int DepartmentId { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }

        public virtual DimCity City { get; set; }
    }

    public class DimClient
    {
        public int ClientKey { get; set; }
        public int ClientId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class DimApartment
    {
        public int ApartmentKey { get; set; }
        public int ApartmentId { get; set; }
        public int ClientId { get; set; }
        public int CityId { get; set; }
        public int RoomsCount { get; set; }
        public string Address { get; set; }

        public virtual DimClient Client { get; set; }
        public virtual DimCity City { get; set; }
    }

    public class DimRoom
    {
        public int RoomKey { get; set; }
        public int RoomId { get; set; }
        public int ApartmentId { get; set; }
        public string RoomType { get; set; }
        public decimal FloorSquare { get; set; }
        public decimal WallsSquare { get; set; }
        public decimal CellingSquare { get; set; }
        public int DoorsCount { get; set; }
        public int WindowsCount { get; set; }

        public virtual DimApartment Apartment { get; set; }
    }

    public class DimRepair
    {
        public int RepairKey { get; set; }
        public int RepairId { get; set; }
        public string RepairType { get; set; }
        public string RepairName { get; set; }
        public string RepairDescription { get; set; }
        public decimal MinRepairHours { get; set; }
        public decimal MaxRepairHours { get; set; }
        public decimal RepairPrice { get; set; }
    }

    public class DimContract
    {
        public int ContractKey { get; set; }
        public int ContractId { get; set; }
        public int ContractStartDateId { get; set; }
        public int? ContractEndDateId { get; set; }
        public int DepartmentId { get; set; }
        public decimal Prepayment { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal WorkEstimatedPrice { get; set; }
        public int ExaminationDateId { get; set; }
    }

    public class RepairServicesFact
    {
        public int RepairServicesKey { get; set; }
        public int RepairServicesId { get; set; }
        public int RepairStartDateId { get; set; }
        public int? RepairEndDateId { get; set; }
        public int RepairId { get; set; }
        public int RoomId { get; set; }
        public int ContractId { get; set; }
        public int RepairCount { get; set; }
        public decimal RepairTotalHours { get; set; }
        public decimal RepairServiceTotalPrice { get; set; }
        public decimal RelationToTotalContractHours { get; set; }
        public decimal RelationToTotalContractPrice { get; set; }
        public int? EmployeeCount { get; set; }
    }

}
