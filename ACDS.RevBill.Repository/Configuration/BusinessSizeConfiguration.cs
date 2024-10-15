using System;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACDS.RevBill.Repository.Configuration
{
    public class BusinessSizeConfiguration : IEntityTypeConfiguration<BusinessSize>
    {
        public void Configure(EntityTypeBuilder<BusinessSize> builder)
        {
            builder.HasData
            (
                new BusinessSize
                {
                    Id = 1,
                    OrganisationId = 1,
                    BusinessSizeName = "Small",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new BusinessSize
                {
                    Id = 2,
                    OrganisationId = 1,
                    BusinessSizeName = "Medium",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new BusinessSize
                {
                    Id = 3,
                    OrganisationId = 1,
                    BusinessSizeName = "Large",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                }
            );
        }
    }
}

