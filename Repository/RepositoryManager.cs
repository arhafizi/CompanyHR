using Contracts;

namespace Repository;
public class RepositoryManager : IRepositoryManager {
    private readonly RepositoryContext _repositoryContext;
    private ICompanyRepository _companyRepository;
    private IEmployeeRepository _employeeRepository;
    public RepositoryManager(RepositoryContext repositoryContext) {
        _repositoryContext = repositoryContext;
    }
    public ICompanyRepository Company {
        get {
            _companyRepository ??= new
                CompanyRepository(_repositoryContext);
            return _companyRepository;
        }
    }
    public IEmployeeRepository Employee {
        get {
            _employeeRepository ??= new
                EmployeeRepository(_repositoryContext);
            return _employeeRepository;
        }
    }
    public void Save() => _repositoryContext.SaveChanges();

}