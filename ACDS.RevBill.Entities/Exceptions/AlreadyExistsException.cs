using System;
namespace ACDS.RevBill.Entities.Exceptions
{
	public abstract class AlreadyExistsException : Exception
    {
        protected AlreadyExistsException(string message)
          : base(message)
        { }
    }
}

