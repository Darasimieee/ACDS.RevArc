using System;
namespace ACDS.RevBill.Entities.Exceptions
{
    public class CustomerExistsException : AlreadyExistsException
    {
        public CustomerExistsException(string email)
           : base($"The customer with email: {email} already exist in the database.")
        {
        }
    }
}

