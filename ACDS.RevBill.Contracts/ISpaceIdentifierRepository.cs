using System;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Shared.RequestFeatures;

namespace ACDS.RevBill.Contracts
{
	public interface ISpaceIdentifierRepository
	{
        Task<IEnumerable<SpaceIdentifier>> GetAllSpaceIdentifiersAsync(int organisationId, bool trackChanges);
        Task<SpaceIdentifier> GetSpaceIdentifierAsync(int organisationId, int spaceIdentifierId, bool trackChanges);
        void CreateSpaceIdentifier(int organisationId, SpaceIdentifier spaceIdentifier);
    }
}