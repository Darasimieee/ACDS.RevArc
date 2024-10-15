using System;
namespace ACDS.RevBill.Presentation.ActionFilters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AllowAnonymousLoginAttribute : Attribute { }
}

