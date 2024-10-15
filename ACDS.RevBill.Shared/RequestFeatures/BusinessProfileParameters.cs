using System;
namespace ACDS.RevBill.Shared.RequestFeatures
{
	public class BusinessProfileParameters : RequestParameters
    {
        public string? Property { get; set; }
        public string? Revenue { get; set; }
    }
}