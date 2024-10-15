using System;
namespace ACDS.RevBill.Entities.Models
{
	public class Property_Customer
	{
		public int PropertyID { get; set; }
        public int CustomerID { get; set; }
		public string PropertyName { get; set; }
        public string CustomerName { get; set; }
    }
}