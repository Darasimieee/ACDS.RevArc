using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record GetFrequencyDto(int Id, string FrequencyName, int Frequency);
}