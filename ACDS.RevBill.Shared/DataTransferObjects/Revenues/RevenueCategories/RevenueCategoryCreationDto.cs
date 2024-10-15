using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects.Revenues.RevenueCategories
{
    public class RevenueCategoryCreationDto
    {
        [Required(ErrorMessage = "Organisation Id is a required field.")]
        public int OrganisationId { get; init; }
        [Required(ErrorMessage = "Category Name is a required field.")]
        public string? CategoryName { get; init; }

        [Required(ErrorMessage = "Category Id is a required field.")]
        public int CategoryId { get; init; }
        [Required(ErrorMessage = "Revenue Id is a required field.")]
        public int RevenueId { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Status is a required field.")]
        public bool Active { get; init; }

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; init; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; init; }
    }
}

