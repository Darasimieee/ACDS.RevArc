using System;
namespace ACDS.RevBill.Entities.Exceptions
{
    public sealed class ModuleExistsException : NotFoundException
    {
        public ModuleExistsException(string Name)
            : base($"The Module with name: {Name} already exists.")
        {
        }
    }
}

