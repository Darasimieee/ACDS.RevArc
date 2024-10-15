using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class UpdateRoleModMenusDto
    {

        [Required(ErrorMessage = "Role Id is a required field.")]
        public int RoleId { get; init; }
        public List<CreateMenusDto> MenusDto { get; init; }
        [Required(ErrorMessage = "Date Modified is a required field.")]
        public DateTime DateModified { get; init; }

        [Required(ErrorMessage = "ModifiedBy is a required field.")]
        public string? ModifiedBy { get; init; }       
    }
}

