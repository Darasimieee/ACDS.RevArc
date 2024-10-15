using System;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;

namespace ACDS.RevBill.Shared.DataTransferObjects.Customer
{
    public record GetCustomerDto(int CustomerId, int OrganisationId, PayerTypeDto PayerTypes,  string PayerId, TitleDto Titles,
        string CorporateName, string FirstName, string LastName, string MiddleName, string FullName, string Email, GenderDto Genders, MaritalStatusDto MaritalStatuses,
        string Address, string PhoneNo);
}