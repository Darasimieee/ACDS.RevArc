using System;
using ACDS.RevBill.Shared.DataTransferObjects.Customer;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.Business_Profile;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration.Property;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
    public class PartialEnumerationParams
    {
        public List<CreateBusinessProfileMultiRevenueDto> CreateBusinessProfileDto { get; set; }
        public CreateCustomerDto? CreateCustomerDto { get; set; }
        public CreateCustomerPropertyDto CreateCustomerPropertyDto { get; set; }
    }
}

