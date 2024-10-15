using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
    public class Category : EntityBase
    {
        [Key]
        public int CategoryId { get; set; }
        public int OrganisationId { get; set; }
        [ForeignKey("BusinessSize")]
        public int BusinessSizeId { get; set; }
        public string? CategoryName { get; set; }
        public int?  PayerTypeId { get; set; }
        public string? Description { get; set; }
        public bool Active { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }

        public BusinessSize? BusinessSize { get; set; }
    }
}