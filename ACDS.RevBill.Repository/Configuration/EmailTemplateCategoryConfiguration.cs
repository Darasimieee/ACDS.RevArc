using System;
using ACDS.RevBill.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ACDS.RevBill.Repository.Configuration
{
    public class EmailTemplateCategoryConfiguration : IEntityTypeConfiguration<EmailTemplateCategory>
    {
        public void Configure(EntityTypeBuilder<EmailTemplateCategory> builder)
        {
            builder.HasData
            (
                new EmailTemplateCategory
                {
                    EmailTemplateCategoryId = 1,
                    Name = "Account Activation",
                    Description = "Email template requesting a user to activate their account",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Sonuga Ayomide"
                },
                new EmailTemplateCategory
                {
                    EmailTemplateCategoryId = 2,
                    Name = "Forgot Password",
                    Description = "Email template that sends an OTP when a user requests a password change",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Sonuga Ayomide"
                },
                new EmailTemplateCategory
                {
                    EmailTemplateCategoryId = 3,
                    Name = "Password Update",
                    Description = "Email template that informs a user that their password has been succesfully changed",
                    DateCreated = DateTime.Now,
                    CreatedBy = "Sonuga Ayomide"
                }
            );
        }
    }
}

