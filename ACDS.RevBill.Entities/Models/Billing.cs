using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Entities.Models
{
    public class Billing : EntityBase
    {
        [Key]
        public long BillId { get; set; }
        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }
        public string? BillReferenceNo { get; set; }
        public string? HarmonizedBillReferenceNo { get; set; }
        [ForeignKey("Property")]
        public int? PropertyId { get; set; }
        [ForeignKey("Customers")]
        public int CustomerId { get; set; }
        [ForeignKey("Agencies")]
        public int AgencyId { get; set; }
        [ForeignKey("BusinessSize")]
        public int BusinessSizeId { get; set; }
        [ForeignKey("Revenues")]
        public int RevenueId { get; set; }
        [Precision(18, 2)]
        public decimal BillAmount { get; set; }
        [Precision(18, 2)]
        public decimal BillArrears { get; set; }
        [Precision(18, 2)]
        public decimal Billbf { get; set; }
        [Precision(18, 2)]
        public decimal AmountPaid { get; set; }
        [ForeignKey("Frequencies")]
        public int? FrequencyId { get; set; }
        [ForeignKey("BillType")]
        public int BillTypeId { get; set; }
        [ForeignKey("BusinessType")]
        public int BusinessTypeId { get; set; }
        public string? AppliedDate { get; set; }
        public string? Category { get; set; }
        [ForeignKey("BillStatus")]
        public int BillStatusId { get; set; }
        public int Year { get; set; }
        public DateTime DateCreated { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? DateModified { get; set; }
        public string? ModifiedBy { get; set; }
        public Organisation? Organisations { get; set; }
        public Customers? Customers { get; set; }
        public Agencies? Agencies { get; set; }
        public Revenues? Revenues { get; set; }
        public BillStatus? BillStatus { get; set; }
        public Frequencies? Frequencies { get; set; }
        public BillType? BillType { get; set; }
        public BusinessType? BusinessType { get; set; }
        public BusinessSize? BusinessSize { get; set; }
        public Property? Property { get; set; }
        public string? HarmonizeStore { get; set; }
    }
}