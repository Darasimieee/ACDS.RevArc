using System;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACDS.RevBill.Repository.Configuration
{
	public class BillTypeConfiguration : IEntityTypeConfiguration<BillType>
    {
        public void Configure(EntityTypeBuilder<BillType> builder)
        {
            builder.HasData
            (
                new BillType
                {
                    Id = 1,
                    BillTypeName = "Property"
                    
                },
                new BillType
                {
                    Id = 2,
                    BillTypeName = "Non-Property",
                   
                }                                      
            );
        }
    }
}

