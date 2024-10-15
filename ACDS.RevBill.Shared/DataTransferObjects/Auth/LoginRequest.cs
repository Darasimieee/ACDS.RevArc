using System;
using System.ComponentModel.DataAnnotations;

namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record class LoginRequest([Required] string Identifier, [Required] string Password);
}