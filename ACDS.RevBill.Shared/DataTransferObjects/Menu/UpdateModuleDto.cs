using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record UpdateModuleDto(string ModuleName, string ModuleCode, DateTime DateModified, string ModifiedBy, bool Active);

}

