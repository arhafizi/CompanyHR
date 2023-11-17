using Entities.Models;
using Shared.DataTransferObjects;

namespace Services.Contracts;
public interface IEmployeeService {
    IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
    EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges);
    EmployeeDto CreateEmployeeForCompany(Guid companyId,
        EmployeeCreationDto employeeForCreation, bool trackChanges);
    void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges);
    void UpdateEmployeeForCompany(Guid companyId, Guid id,
        EmployeeUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);
    (EmployeeUpdateDto empToPatch, Employee empEntity) GetEmployeeForPatch(Guid companyId,
        Guid id, bool compTrackChanges, bool empTrackChanges);
    void SaveChangesForPatch(EmployeeUpdateDto empPatch, Employee empEntity);
}