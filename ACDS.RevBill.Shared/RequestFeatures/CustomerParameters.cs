using System;
namespace ACDS.RevBill.Shared.RequestFeatures
{
	public class CustomerParameters : RequestParameters
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PayerId { get; set; }
    }
}