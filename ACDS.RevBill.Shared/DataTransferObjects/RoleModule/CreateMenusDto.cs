using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class CreateMenusDto
    {
        [Required(ErrorMessage = "Module Id is a required field.")]
        public int ModuleId { get; init; }
        [Required(ErrorMessage = "Menu Id is a required field.")]
        public List<int> MenuIds { get; init; }
       
      
    }
}

