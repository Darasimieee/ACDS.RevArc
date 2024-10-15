namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record GetModuleDto(int ModuleId, string ModuleName, string ModuleCode, bool Active);
}
