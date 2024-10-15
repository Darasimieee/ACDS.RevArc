namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record GetRoleModMenuDto(int OrganisationId, int RoleModuleMenusId, RoleDto Roles, GetModuleDto Modules,GetMenuDto Menus);
}