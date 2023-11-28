using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository;
public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository {
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

    public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,
        EmployeeParameters empParameters, bool trackChanges) {

        var employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            .OrderBy(e => e.Name)
            .Skip((empParameters.PageNumber - 1) * empParameters.PageSize)
            .Take(empParameters.PageSize)
            .ToListAsync();
        //additional call to db - for big dbs
        var count = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
            .CountAsync();

        return new 
            PagedList<Employee>(employees, count, empParameters.PageNumber, empParameters.PageSize);
    }

    public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) {

        return await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();
    }

    public void CreateEmployeeForCompany(Guid companyId, Employee employee) {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee) {
        Delete(employee);
    }
}
