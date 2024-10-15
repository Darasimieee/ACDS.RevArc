using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDS.RevBill.Repository.Configuration
{
    public class MenusConfiguration : IEntityTypeConfiguration<Menus>
    {
        public void Configure(EntityTypeBuilder<Menus> builder)
        {
            builder.HasData
            (
                new Menus
                {
                    MenuId = 1,
                    MenuName = "Services",
                    ModuleCode = "ser11",
                    Active = true,
                    DateCreated = DateTime.Now,
                    CreatedBy = "Lilian Aduku"
                }
            );
        }
    }

}
