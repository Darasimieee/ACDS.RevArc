using System;
namespace ACDS.RevBill.Entities.Exceptions
{
    public sealed class IdNotFoundException : NotFoundException
    {
        public IdNotFoundException(string controller, int Id)
            : base($"The {controller} with id: {Id} doesn't exist in the database.")
        {
        }
    }
}

