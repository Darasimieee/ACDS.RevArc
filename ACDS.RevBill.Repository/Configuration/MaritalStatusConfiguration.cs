using System;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACDS.RevBill.Repository.Configuration
{
	public class MaritalStatusConfiguration : IEntityTypeConfiguration<MaritalStatuses>
    {
        public void Configure(EntityTypeBuilder<MaritalStatuses> builder)
        {
            builder.HasData
            (
                new MaritalStatuses
                {
                    MaritalStatusId = 1,
                    MaritalStatusCode = "S",
                    MaritalStatusName = "Single/Separated",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new MaritalStatuses
                {
                    MaritalStatusId = 2,
                    MaritalStatusCode = "M",
                    MaritalStatusName = "Married",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                }
            );
        }
    }
}

