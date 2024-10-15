using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class Roles
	{
        [Key]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Role name is a required field.")]
        public string? RoleName { get; set; }
        [Required(ErrorMessage = "Status is a required field.")]
        public bool? Status { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}

