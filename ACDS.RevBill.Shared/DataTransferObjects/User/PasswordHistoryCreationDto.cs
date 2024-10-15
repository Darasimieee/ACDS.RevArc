using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.User
{
	public class PasswordHistoryCreationDto
	{
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*_#?&])[A-Za-z\d@$!%*_^()=#`~+<>,./\/|}{[?&]{8,}$",
            ErrorMessage = "The Password must be at least 8 characters long and have at least a special character, number and have a lower and upper case letter.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime? DateCreated { get; set; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; set; }
    }
}

