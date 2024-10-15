namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record GetOrganisationModuleDto(int OrganisationModuleId, GetModuleDto Modules, int OrganisationId, int Status);
}