using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Entities.Models
{
	public class Titles
	{
        [Key]
        public int TitleId { get; set; }
        public string? TitleCode { get; set; }
        public string? TitleName { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}

