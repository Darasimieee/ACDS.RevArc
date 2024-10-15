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
    public class EmailAccountConfiguration : IEntityTypeConfiguration<EmailAccounts>
    {
        public void Configure(EntityTypeBuilder<EmailAccounts> builder)
        {
            builder.HasData
            (
                new EmailAccounts
                {
                    //LgaId = 1,
                    //CountryId = 129,
                    //LGAName = "Alimosho",
                    //StateId = 24,
                    //LgaStatus = true,
                    //DateCreated = DateTime.Now,
                    //CreatedBy = "Lilian Aduku",

                }
            );
        }
    }

}
