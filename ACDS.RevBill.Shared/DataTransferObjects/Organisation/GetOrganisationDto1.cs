namespace ACDS.RevBill.Shared.DataTransferObjects
{
	public class GetOrganisationDto1
	{
		public int OrganisationId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public int LgaId { get; set; }
        public int LcdaId { get; set; }
        public string PayerId { get; set; }
        public string OrganisationName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string WebUrl { get; set; }
        public int OrganisationApprovalStatus { get; set; }
        public bool OrganisationStatus { get; set; }
    }
}