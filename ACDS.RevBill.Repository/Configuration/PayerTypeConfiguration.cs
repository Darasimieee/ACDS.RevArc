using System;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACDS.RevBill.Repository.Configuration
{
	public class PayerTypeConfiguration : IEntityTypeConfiguration<PayerTypes>
    {
        public void Configure(EntityTypeBuilder<PayerTypes> builder)
        {
            builder.HasData
            (
                new PayerTypes
                {
                    PayerTypeId = 1,
                    PayerTypeCode = "N",
                    PayerTypeName = "Individual",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new PayerTypes
                {
                    PayerTypeId = 2,
                    PayerTypeCode = "C",
                    PayerTypeName = "Corporate",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                }
            );
        }
    }
}

