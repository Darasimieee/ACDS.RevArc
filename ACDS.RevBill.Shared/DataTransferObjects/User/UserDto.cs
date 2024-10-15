using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
	public record UserDto(int UserId, int OrganisationId, string UserName, string Email, string PhoneNumber, bool AccountConfirmed, bool LockoutEnabled,
		bool Active);
}

