using Entities.LinkModels;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Dynamic;

namespace Services.Contracts;
public interface IEmployeeService {
    Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync(
        Guid companyId, LinkParameters linkParameters, bool trackChanges);
    Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
    Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId,
        EmployeeCreationDto employeeForCreation, bool trackChanges);
    Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges);
    Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id,
        EmployeeUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges);
    Task<(EmployeeUpdateDto empToPatch, Employee empEntity)> GetEmployeeForPatchAsync(Guid companyId,
        Guid id, bool compTrackChanges, bool empTrackChanges);
    Task SaveChangesForPatchAsync(EmployeeUpdateDto empPatch, Employee empEntity);
}