using System;
using ACDS.RevBill.Entities.Models;

namespace ACDS.RevBill.Shared.DataTransferObjects.User
{
    public class UserProfileDto
    {
        public int UserProfileId { get; set; }
        public string? RoleName { get; set; }
        public string? AgencyName { get; set; }
        public Organisation? Organisation { get; set; }
        public string? Surname { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool Confirmed { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
    }
}