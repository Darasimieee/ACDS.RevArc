using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.Enumeration
{
	public class LcdaDto
	{
        public int Id { get; set; }
        public string? LCDAName { get; set; }
        //public string? EbsLgaId { get; set; }
        public string? EbsLcdaId { get; set; }
        public string? LGAId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
    }
}

