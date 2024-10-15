using System;
namespace ACDS.RevBill.Entities.Exceptions
{
    public sealed class MaxDateRangeBadRequestException : BadRequestException
    {
        public MaxDateRangeBadRequestException()
        : base("End Date can't be less than Start Date.")
        {
        }
    }
}