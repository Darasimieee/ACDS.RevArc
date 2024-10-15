using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record GetMenuDto(int MenuId, string MenuName, int ModuleId, bool Active);
}

