using System;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACDS.RevBill.Repository.Configuration
{
	public class GenderConfiguration : IEntityTypeConfiguration<Genders>
    {
        public void Configure(EntityTypeBuilder<Genders> builder)
        {
            builder.HasData
            (
                new Genders
                {
                    GenderId = 1,
                    GenderCode = "M",
                    GenderName = "Male",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new Genders
                {
                    GenderId = 2,
                    GenderCode = "F",
                    GenderName = "Female",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                }
            );
        }
    }
}

