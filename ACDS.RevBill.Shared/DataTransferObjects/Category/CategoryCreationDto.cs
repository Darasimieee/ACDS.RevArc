﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public class CategoryCreationDto
    {
        [Required(ErrorMessage = "Organisation Id is a required field.")]
        public int OrganisationId { get; init; }
        [Required(ErrorMessage = "BusinessSize Id is a required field.")]
        public int BusinessSizeId { get; init; }
        [Required(ErrorMessage = "PayerType Id is a required field.")]
        public int PayerTypeId { get; init; }
        [Required(ErrorMessage = "Category Name is a required field.")]
        public string? CategoryName { get; init; }
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

