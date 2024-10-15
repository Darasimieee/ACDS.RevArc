using System;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACDS.RevBill.Repository.Configuration
{
	public class FrequencyConfiguration : IEntityTypeConfiguration<Frequencies>
    {
        public void Configure(EntityTypeBuilder<Frequencies> builder)
        {
            builder.HasData
            (
                new Frequencies
                {
                    Id = 1,
                    FrequencyName = "Daily",
                    Frequency = 1,
                    DateCreated = DateTime.Now,
                    CreatedBy = "Lilian Aduku ",
                },
                new Frequencies
                {
                    Id = 2,
                    FrequencyName = "Weekly",
                    Frequency = 7,
                    DateCreated = DateTime.Now,
                    CreatedBy = "Lilian Aduku ",
                },
                new Frequencies
                {
                    Id = 3,
                    FrequencyName = "Monthly",
                    Frequency = 31,
                    DateCreated = DateTime.Now,
                    CreatedBy = "Lilian Aduku ",
                },
                new Frequencies
                {
                    Id = 4,
                    FrequencyName = "Quarterly",
                    Frequency = 90,
                    DateCreated = DateTime.Now,
                    CreatedBy = "Lilian Aduku ",
                },
                new Frequencies
                {
                    Id = 5,
                    FrequencyName = "Bi-Annually",
                    Frequency = 183,
                    DateCreated = DateTime.Now,
                    CreatedBy = "Lilian Aduku ",
                },
                new Frequencies
                {
                    Id = 6,
                    FrequencyName = "Annually",
                    Frequency = 1,
                    DateCreated = DateTime.Now,
                    CreatedBy = "Lilian Aduku ",
                }
                
            );
        }
    }
}

