using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Entities.Models
{
	public class MaritalStatuses
	{
        [Key]
        public int MaritalStatusId { get; set; }
        public string? MaritalStatusCode { get; set; }
        public string? MaritalStatusName { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}

