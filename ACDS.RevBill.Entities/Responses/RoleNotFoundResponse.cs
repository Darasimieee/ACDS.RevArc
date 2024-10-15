namespace ACDS.RevBill.Entities.Responses;

public sealed class RoleNotFoundResponse : ApiNotFoundResponse
{
	public RoleNotFoundResponse(Guid id)
		: base($"Role with id: {id} is not found in db.")
	{
	}
}
