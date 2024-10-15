using System;
namespace ACDS.RevBill.Entities.Exceptions
{
    public sealed class CustomerEmailNotFoundException : NotFoundException
    {
        public CustomerEmailNotFoundException(string email)
            : base($"The customer with email: {email} doesn't exist in the database.")
        {
        }

    }
}

