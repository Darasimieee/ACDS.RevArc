using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenuePrices
{
  
    public class RevenuePriceListDto
    {
        public int RevenuePriceId { get; set; }
        public int OrganisationId { get; init; }
        public string? CategoryName { get; init; }
        public int CategoryId { get; init; }
        public int RevenueId { get; set; }
        public string BusinessSize { get; set; }
        public decimal Amount { get; set; }
        public bool Active { get; init; }
        public DateTime DateCreated { get; init; }
        public string? CreatedBy { get; init; }
        public DateTime DateModified { get; init; }
        public string? ModifiedBy { get; init; }
    }

}

