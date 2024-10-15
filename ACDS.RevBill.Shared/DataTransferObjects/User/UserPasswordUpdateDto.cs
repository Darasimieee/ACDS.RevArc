using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.User
{
    public class UserPasswordUpdateDto
    {
        [Required]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*_#?&])[A-Za-z\d@$!%*_^()=#`~+<>,./\/|}{[?&]{8,}$",
            ErrorMessage = "The Password must be at least 8 characters long and have at least a special character, number and have a lower and upper case letter.")]
        [DataType(DataType.Password)]
        public string? Password { get; init; }

        [Required(ErrorMessage = "DateModified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; init; }
    }
}

