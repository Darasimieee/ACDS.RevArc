using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Repository.Extensions;
using ACDS.RevBill.Shared.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace ACDS.RevBill.Repository
{
    internal sealed class DebtRepository : RepositoryBase<Payment>, IDebtRepository
    {
        public DebtRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }

         }
}