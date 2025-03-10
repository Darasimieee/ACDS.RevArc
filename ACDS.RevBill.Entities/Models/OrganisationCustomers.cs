using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Entities.Models
{
    public class OrganisationCustomers: EntityBase
    {
        [Key]
        public int OrganisationCustomerId { get; set; }
        //[ForeignKey("Customers")]
        //public int CustomerId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
        //public Customers? Customers { get; set; }
    }
}
