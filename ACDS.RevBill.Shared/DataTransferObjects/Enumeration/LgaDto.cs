using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class LgaDto
	{
        public int Id { get; set; }
        public string? LGAName { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
    }
}

