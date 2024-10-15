using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
    public interface IBillFormatRepository
	{
        Task<PagedList<BillFormat>> GetAllBillFormatsAsync(int organisationId, DefaultParameters requestParameters, bool trackChanges);
        Task<BillFormat> GetBillFormatAsync(int organisationId, int billFormatId, bool trackChanges);
        void CreateBillFormat(int organisationId, BillFormat billFormat);
    }
}