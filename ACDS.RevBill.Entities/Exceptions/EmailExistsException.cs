using System;
namespace ACDS.RevBill.Entities.Exceptions
{
	public class EmailExistsException : AlreadyExistsException
    {
        public EmailExistsException(string Email)
           : base($"The user with email: {Email} already exist in the database.")
        {
        }
    }
}

