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
}