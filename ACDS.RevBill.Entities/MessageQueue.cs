using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Entities
{
	public class MessageQueue
	{
        public long Id { get; set; }
        public string? Message { get; set; }
        public string? ToEmail { get; set; }
        public int Status { get; set; }
        public string? Subject { get; set; }
        public DateTime SendDate { get; set; }
        public string? Phone { get; set; }
        public virtual bool IsSuccessfull { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? GeneratedDate { get; set; }
    }
}

