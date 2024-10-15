using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record CreateMenuDto(string MenuName, DateTime DateCreated, string CreatedBy, bool Active);
}