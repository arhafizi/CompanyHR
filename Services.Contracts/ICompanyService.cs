using Shared.DataTransferObjects;

namespace Services.Contracts;
public interface ICompanyService {
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
    CompanyDto GetCompany(Guid companyId, bool trackChanges);
    CompanyDto CreateCompany(CompanyCreationDto company);
    IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges);
    (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(
        IEnumerable<CompanyCreationDto> companyCollection);
    void DeleteCompany(Guid companyId, bool trackChanges);

    void UpdateCompany(Guid companyid, CompanyUpdateDto companyForUpdate, bool trackChanges);
}
