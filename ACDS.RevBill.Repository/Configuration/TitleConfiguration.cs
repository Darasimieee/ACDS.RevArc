using System;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACDS.RevBill.Repository.Configuration
{
	public class TitleConfiguration : IEntityTypeConfiguration<Titles>
    {
        public void Configure(EntityTypeBuilder<Titles> builder)
        {
            builder.HasData
            (
                new Titles
                {
                    TitleId = 1,
                    TitleCode = "Mr.",
                    TitleName = "Mister",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new Titles
                {
                    TitleId = 2,
                    TitleCode = "Mrs.",
                    TitleName = "Missis",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new Titles
                {
                    TitleId = 3,
                    TitleCode = "Miss.",
                    TitleName = "Miss",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new Titles
                {
                    TitleId = 4,
                    TitleCode = "Master",
                    TitleName = "Master",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new Titles
                {
                    TitleId = 5,
                    TitleCode = "Dr.",
                    TitleName = "Doctor",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new Titles
                {
                    TitleId = 6,
                    TitleCode = "Alh.",
                    TitleName = "Alhaji",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new Titles
                {
                    TitleId = 7,
                    TitleCode = "Alh.",
                    TitleName = "Alhaja",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                }
            );
        }
    }
}

