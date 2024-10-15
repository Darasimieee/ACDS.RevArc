using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Entities.Models
{
	public class BillStatus
	{
        [Key]
        public int BillStatusId { get; set; }
        public string? BillStatusName { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}

