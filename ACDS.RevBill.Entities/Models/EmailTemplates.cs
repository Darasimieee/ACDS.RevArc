using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class EmailTemplates : EntityBase
    {
        [Key]
        public int EmailTemplateId { get; set; }
        public string? Name { get; set; }
        public string? EmailTemplate { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }

        [ForeignKey(nameof(EmailAccounts))]
        public int EmailAccountId { get; set; }
        [ForeignKey(nameof(EmailTemplateCategory))]
        public int EmailTemplateCategoryId { get; set; }
    }
}

