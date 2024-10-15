using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Entities.Models
{
	public class PayerTypes
	{
        [Key]
        public int PayerTypeId { get; set; }
        public string? PayerTypeCode { get; set; }
        public string? PayerTypeName { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}

