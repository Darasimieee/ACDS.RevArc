using System;
using ACDS.RevBill.Shared.DataTransferObjects.Customer;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration.Property
{
    public record GetCustomerPropertyDto(int CustomerPropertyId, int CustomerId, GetPropertiesDto Property, GetCustomerDto Customers);
}