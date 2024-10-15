using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;


namespace ACDS.RevBill.Contracts
{
    public interface ILgasRepository
    {
        Task<IEnumerable<Lgas>> GetAllLgasAsync(bool trackChanges);
    }
}
