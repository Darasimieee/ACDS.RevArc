using System;
using ACDS.RevBill.Entities.Models;

namespace ACDS.RevBill.Contracts
{
	public interface IPasswordHistoryRepository
    {
        void AddPassword(int UserId, PasswordHistory passwordHistory);
    }
}