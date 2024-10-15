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
    public class SmsAccountConfiguration : IEntityTypeConfiguration<SmsAccounts>
    {
        public void Configure(EntityTypeBuilder<SmsAccounts> builder)
        {
            builder.HasData
            (
                new SmsAccounts
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
