using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record UpdateMenuDto(string MenuName, string ModuleCode, DateTime DateModified, string ModifiedBy, bool Active);

}

