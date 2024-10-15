using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class CreateRoleModMenusDto
    {

        [Required(ErrorMessage = "Role Id is a required field.")]
        public int RoleId { get; init; }
        public List<CreateMenusDto> MenusDto { get; init; }
        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; init; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; init; }       
    }
}

