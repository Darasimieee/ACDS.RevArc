namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record GetRoleModuleDto(int OrganisationId, int RoleModuleId, RoleDto Roles, GetModuleDto Modules);
}