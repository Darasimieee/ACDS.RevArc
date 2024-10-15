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
    public class SmsTemplateConfiguration : IEntityTypeConfiguration<SmsTemplates>
    {
        public void Configure(EntityTypeBuilder<SmsTemplates> builder)
        {
            builder.HasData
            (
                new SmsTemplates
                {
                    //Name = "UserCreation",
                    //BccEmailAddress = " ",
                    //ToEmailAddress = " ",
                    //ToEmailGroupName = " ",
                    //Subject= "User Creation",
                    //Body="Dear  ,<br><br> You have been created on RevBill portal below is your credentials <br><br> ",
                    //Active = true,
                    //DateCreated = DateTime.Now,
                    //CreatedBy = "Lilian Aduku",
                    //EmailAccountId =1


                }
            );
        }
    }

}
