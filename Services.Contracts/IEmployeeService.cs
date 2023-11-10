using Shared.DataTransferObjects;

namespace Services.Contracts;
public interface IEmployeeService {
    IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
}

