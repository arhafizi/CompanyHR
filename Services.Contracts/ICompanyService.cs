using Shared.DataTransferObjects;

namespace Services.Contracts;
public interface ICompanyService {
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
    CompanyDto GetCompany(Guid companyId, bool trackChanges);

}
