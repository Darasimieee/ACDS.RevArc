using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Entities.Models
{
	public class Genders
	{
		[Key]
		public int GenderId { get; set; }
		public string? GenderCode { get; set; }
		public string? GenderName { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}

