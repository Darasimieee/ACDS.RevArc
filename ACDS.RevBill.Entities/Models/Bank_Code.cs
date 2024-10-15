using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Entities.Models
{
	public class Bank_Code
	{
        [Key]
        public int Id { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
       
    }
}