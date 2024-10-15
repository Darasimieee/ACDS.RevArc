using System;
using ACDS.RevBill.Shared.DataTransferObjects.User;

namespace ACDS.RevBill.Shared.DataTransferObjects.Auth
{
	public class UpdatePasswordParams
	{
        public UserPasswordUpdateDto? UserPasswordUpdateDto { get; set; }
        public PasswordHistoryCreationDto? PasswordHistoryCreationDto { get; set; }
    }
}

