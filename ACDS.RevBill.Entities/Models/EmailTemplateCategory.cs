using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Entities.Models
{
    public class EmailTemplateCategory 
    {
        [Key]
        public int EmailTemplateCategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}

