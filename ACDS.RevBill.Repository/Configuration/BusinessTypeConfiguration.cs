using System;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACDS.RevBill.Repository.Configuration
{
    public class BusinessTypeConfiguration : IEntityTypeConfiguration<BusinessType>
    {
        public void Configure(EntityTypeBuilder<BusinessType> builder)
        {
            builder.HasData
            (
                new BusinessType
                {
                    Id = 1,
                    OrganisationId = 1,
                    BusinessTypeName = "Banking",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new BusinessType
                {
                    Id = 2,
                    OrganisationId = 1,
                    BusinessTypeName = "Trading",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new BusinessType
                {
                    Id = 3,
                    OrganisationId = 1,
                    BusinessTypeName = "Foodstuff",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new BusinessType
                {
                    Id = 4,
                    OrganisationId = 1,
                    BusinessTypeName = "Tailoring",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new BusinessType
                {
                    Id = 5,
                    OrganisationId = 1,
                    BusinessTypeName = "Hotel",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new BusinessType
                {
                    Id = 6,
                    OrganisationId = 1,
                    BusinessTypeName = "Bar",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                }
            );
        }
    }
}