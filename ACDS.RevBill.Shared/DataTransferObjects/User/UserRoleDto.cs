using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.User
{
    public record UserRoleDto(int UserRoleId, int OrganisationId, int RoleId, RoleDto Roles);
}