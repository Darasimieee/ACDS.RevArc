using System;
namespace ACDS.RevBill.Entities
{
	public class Response
	{
        public string? StatusMessage { get; set; }
        public object? Data { get; set; }
        public int Status { get; set; }    
    }
}

