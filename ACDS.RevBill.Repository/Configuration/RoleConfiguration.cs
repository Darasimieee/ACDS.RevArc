using System;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACDS.RevBill.Repository.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.HasData
            (
                new Roles
                {
                    RoleId = 1,
                    RoleName = "Super Admin",
                    Status = true,
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                new Roles
                {
                    RoleId = 2,
                    RoleName = "Admin",
                    Status = true,
                    DateCreated = DateTime.Now,
                    CreatedBy = "Ayomide Sonuga",
                },
                 new Roles
                 {
                     RoleId = 3,
                     RoleName = "Super User",
                     Status = true,
                     DateCreated = DateTime.Now,
                     CreatedBy = "Ayomide Sonuga",
                 },
                 new Roles
                 {
                     RoleId = 4,
                     RoleName = "User",
                     Status = true,
                     DateCreated = DateTime.Now,
                     CreatedBy = "Ayomide Sonuga",
                 },
                       new Roles
                       {
                           RoleId = 5,
                           RoleName = "Admin1",
                           Status = true,
                           DateCreated = DateTime.Now,
                           CreatedBy = "Aduku Lilian",
                       }

            );
        }
    }
}

