using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class UserPasswordResetRequests
    {
        [Key]
        public int UserPasswordResetRequestId { get; set; }
        [ForeignKey(nameof(Users))]
        public int UserId { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public string? OTP { get; set; }
        public string? TenantName { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
    }
}

