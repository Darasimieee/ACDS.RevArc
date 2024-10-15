using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Entities.Models.DTOs
{
    public class AddRequestDto
    {
        [Required(ErrorMessage = "Department Name is a required field.")]
        public string DepartmentName { get; set; }
        [Required(ErrorMessage = "Department Code is a required field.")]
        public string? DepartmentCode { get; set; }
        [Required(ErrorMessage = "Status is a required field.")]
        public bool DepartmentStatus { get; set; }
    }
}
