using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
    public class SmsTemplates : EntityBase
    {
        [Key]
        public int SmsTemplateId { get; set; }
        [StringLength(100)] public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the BCC PhoneNumbers
        /// </summary>
        [StringLength(100)] public string? BccPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        [StringLength(1000)] public string? Subject { get; set; }

        /// <summary>
        /// Gets or sets the body
        /// </summary>
        public string? Body { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the template is active
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// Gets or sets the used sms account identifier
        /// </summary>
        [ForeignKey("SmsAccount")]
        public int SmsAccountId { get; set; }

        public DateTime? DateCreated { get; set; }
        [StringLength(300)] public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        [StringLength(300)] public string? ModifiedBy { get; set; }
    }
}
