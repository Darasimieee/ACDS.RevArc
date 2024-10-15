using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Entities.Models.DTOs
{
    public class DepartmentDto
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public string? DepartmentCode { get; set; }
        public bool DepartmentStatus { get; set; }
    }
}
