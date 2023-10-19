using Entities.Models;

namespace Services.Contracts;
public interface ICompanyService {
    IEnumerable<Company> GetAllCompanies(bool trackChanges);

}
