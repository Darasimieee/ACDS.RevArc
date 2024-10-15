using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Shared.DataTransferObjects.Billing
{
	public class UpdatePropertyBill
	{
        public string? HarmonizedBillReferenceNo { get; set; }
        public List<UpdateBilldto> UpdateBilldto { get; set; }
      
    }
}

