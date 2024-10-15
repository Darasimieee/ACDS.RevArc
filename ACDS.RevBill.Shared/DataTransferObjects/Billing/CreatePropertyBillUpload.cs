using System;
using System.ComponentModel.DataAnnotations;
using ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class CreatePropertyBillUpload
    {

        public string? SpaceIdentifier { get; set; }
        public string? BuildingName    { get; set; }
        public string? BuildingNumber { get; set; }
        public string? StreetName { get; set; }
        public string? SpaceFloor{ get; set; }
        public string? Ward { get; set; }
        public string? PayerType { get; set; }
        public string? PayerID { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Agency { get; set; }
        public string? Revenue{ get; set; }
        public string? RevenueCode { get; set; }
        public string? Category { get; set; }
        public string? BusinessType { get; set; }
        public string? BusinessSize { get; set; }
        public string? AppliedDate { get; set; }
        public string? Interest { get; set; }
        public string? Penalty { get; set; }
    }
}

