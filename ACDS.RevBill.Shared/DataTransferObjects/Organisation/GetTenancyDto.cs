namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class GetTenancyDto
    {
        public int Id { get; set; }
        public string TenantId { get; set; }
        public string ConnectionString { get; set; }
        public string OrganisationName { get; set; }
    }
}