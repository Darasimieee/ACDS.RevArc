using ACDS.RevBill.Entities.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class RevenueCreationDto
    {
        [Required(ErrorMessage = "Organisation Id is a required field.")]
        public int OrganisationId { get; init; }
        [Required(ErrorMessage = "BusinessType Id is a required field.")]
        public int BusinessTypeId { get; set; }
        [Required(ErrorMessage = "Revenue Code is a required field.")]
        public string? RevenueCode { get; init; }
        [Required(ErrorMessage = "Revenue Name is a required field.")]
        public string? RevenueName { get; init; }
        [Required(ErrorMessage = "Description is a required field.")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Status is a required field.")]
        public bool Active { get; init; }

        [Required(ErrorMessage = "Date Created is a required field.")]
        public DateTime DateCreated { get; init; }

        [Required(ErrorMessage = "CreatedBy is a required field.")]
        public string? CreatedBy { get; init; }
    }
}

