using System;
namespace ACDS.RevBill.Shared.DataTransferObjects.User
{
    public record UserPasswordDto(int UserPasswordId, int UserId, string Password, bool Active);
}

