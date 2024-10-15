using System;
using System.ComponentModel.DataAnnotations.Schema;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class BillingReportDto
    {
        public string PropertyName { get; set; }

        public string CategoryName { get; set; }
        
        public string PropertyAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string CustomerAddress { get; set; }
        public string PayerID { get; set; }
        public string? AreaOffice { get; set; }
        public string Summary { get; set; }
        public DateTime GeneratedDate { get; set; }
        public string PrintedDate { get; set; }
        public string Year { get; set; }
        public string BillReference { get; set; }
        public string? HarmonizedBillReference { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Arrears { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance { get; set; }
        public string? OrganisationName { get; set; }
        public string? OrganisationAddress { get; set; }
        public string? OrganisationPhoneNumber { get; set; }
        public string? OrganisationEmail { get; set; }
        public string? ContentOne { get; set; }
        public string? ContentTwo { get; set; }
        public string? ContentThree { get; set; }
        public byte[]? OrganisationLogo { get; set; }
        public byte[]? BarCode { get; set; }
        public byte[]? SignatureOne { get; set; }
        public byte[]? SignatureTwo { get; set; }
        
       
    }
}