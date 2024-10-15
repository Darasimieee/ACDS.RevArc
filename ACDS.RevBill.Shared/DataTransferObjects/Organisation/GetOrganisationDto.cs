using System;
namespace ACDS.RevBill.Shared.DataTransferObjects
{
    public record GetOrganisationDto(int OrganisationId, int CountryId, string PayerId, string OrganisationName, string Address
        , string City, int StateId, int LgaId, int LcdaId, string PhoneNo, string Email, string WebUrl, byte[] LogoData, string LogoName,
        string BillSchema, byte[] BackgroundImagesData, string BackgroundImagesName, bool OrganisationStatus, int OrganisationApprovalStatus);

}