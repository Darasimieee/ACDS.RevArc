using System;
using ACDS.RevBill.Shared.DataTransferObjects.User;

namespace ACDS.RevBill.Shared.DataTransferObjects.Customer
{
	public class CustomerRequestParams
	{
        public UserCreationDto? UserCreationDto { get; set; }
        public UserProfileCreationDto? UserProfileCreationDto { get; set; }
        public UserRoleCreationDto? UserRoleCreationDto { get; set; }
        public UserPasswordCreationDto? UserPasswordCreationDto { get; set; }
        public PasswordHistoryCreationDto? PasswordHistoryCreationDto { get; set; }
    }
}

