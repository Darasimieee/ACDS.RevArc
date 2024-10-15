using System;
namespace ACDS.RevBill.Entities.Exceptions
{
    public class UsernameExistsException : AlreadyExistsException
    {
        public UsernameExistsException(string Username)
           : base($"The user with username: {Username} already exist in the database.")
        {
        }
    }
}

