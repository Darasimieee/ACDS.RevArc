using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Entities.Models
{
    public class SmsAccounts : EntityBase
    {
        [Key]
        public int SmsAccountId { get; set; }
        [StringLength(255)] public string? SmsGateway { get; set; }
        [StringLength(255)] public string? DisplayName { get; set; }
        [StringLength(4000)] public string? SendSmsEndPoint { get; set; }
        [StringLength(255)] public string? Username { get; set; }
        [StringLength(255)] public string? Password { get; set; }
        [StringLength(4000)] public string? BalanceEnqEndPoint { get; set; }
        public bool Active { get; set; }
        public ICollection<SmsTemplates>? SmsTemplates { get; set; } = new List<SmsTemplates>();
        public DateTime? DateCreated { get; set; }
        [StringLength(300)] public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        [StringLength(300)] public string? ModifiedBy { get; set; }
    }
}
