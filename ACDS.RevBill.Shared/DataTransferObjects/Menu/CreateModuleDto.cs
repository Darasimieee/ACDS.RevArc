using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record CreateModuleDto(string ModuleName, string ModuleCode, DateTime DateCreated, string CreatedBy, bool Active);
}

