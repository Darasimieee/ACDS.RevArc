using System;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACDS.RevBill.Repository.Configuration
{
    public class SpaceIdentifierConfiguration : IEntityTypeConfiguration<SpaceIdentifier>
    {
        public void Configure(EntityTypeBuilder<SpaceIdentifier> builder)
        {
            builder.HasData
            (
                new SpaceIdentifier
                {
                    Id = 1,
                    OrganisationId = 1,
                    SpaceIdentifierName = "Building",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new SpaceIdentifier
                {
                    Id = 2,
                    OrganisationId = 1,
                    SpaceIdentifierName = "Floor",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new SpaceIdentifier
                {
                    Id = 3,
                    OrganisationId = 1,
                    SpaceIdentifierName = "Cubicle",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new SpaceIdentifier
                {
                    Id = 4,
                    OrganisationId = 1,
                    SpaceIdentifierName = "Complex",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new SpaceIdentifier
                {
                    Id = 5,
                    OrganisationId = 1,
                    SpaceIdentifierName = "House",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new SpaceIdentifier
                {
                    Id = 6,
                    OrganisationId = 1,
                    SpaceIdentifierName = "Duplex",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new SpaceIdentifier
                {
                    Id = 7,
                    OrganisationId = 1,
                    SpaceIdentifierName = "Flat Room",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                }
            );
        }
    }
}

