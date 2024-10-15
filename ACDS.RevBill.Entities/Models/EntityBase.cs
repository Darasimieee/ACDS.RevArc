using System;
namespace ACDS.RevBill.Entities.Models
{
    public abstract class EntityBase
    {
        public string? TenantName { get; set; }
    }
}