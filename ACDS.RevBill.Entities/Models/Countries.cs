using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Entities.Models
{
    public class Countries
    {
        [Key]
        public int Id { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public string? CapitalCity { get; set; }
        public string? CurrencyName { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CreatedBy { get; set; }
        public int RecordStatus { get; set; }
    }
}