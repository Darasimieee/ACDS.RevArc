using System;
namespace ACDS.RevBill.Entities.Exceptions
{
    public class RoleModuleExistsException : AlreadyExistsException
    {
        public RoleModuleExistsException(int id)
           : base($"The Module with Id: {id} already exist in the role.")
        {
        }
    }
}

