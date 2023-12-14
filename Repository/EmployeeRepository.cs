using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extension;
using Shared.RequestFeatures;

namespace Repository;
public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository {
    public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext) { }

    public async Task<PagedList<Employee>> GetEmployeesAsync(Guid cmpId, EmployeeParameters empPrms, bool tc) {

        var employees = await FindByCondition(e => e.CompanyId.Equals(cmpId), tc)
            .FilterEmployees(empPrms.MinAge, empPrms.MaxAge)
            .Search(empPrms.SearchTerm!)
            .Sort(empPrms.OrderBy!)
            .Skip((empPrms.PageNumber - 1) * empPrms.PageSize)
            .Take(empPrms.PageSize)
            .ToListAsync();

        //additional call to db - for big dbs
        var count = await FindByCondition(e => e.CompanyId.Equals(cmpId), tc).CountAsync();

        return new
            PagedList<Employee>(employees, count, empPrms.PageNumber, empPrms.PageSize);
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
