using System;
namespace ACDS.RevBill.Entities.Exceptions
{
    public sealed class NameFoundException : NotFoundException
    {
        public NameFoundException(string name)
            : base($"The : {name} already exist in the database.")
        {
        }
    }
}

