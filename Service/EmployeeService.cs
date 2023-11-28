using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Services.Contracts;
using Shared.DataTransferObjects;

namespace Service;
internal class EmployeeService : IEmployeeService {
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges) {

        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeesFromDb = await _repository.Employee
            .GetEmployeesAsync(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

        return employeesDto;
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) {

        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if (employeeDb is null)
            throw new EmployeeNotFoundException(id);

        var employee = _mapper.Map<EmployeeDto>(employeeDb);

        return employee;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId,
        EmployeeCreationDto employeeForCreation, bool trackChanges) {

        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeEntity = _mapper.Map<Employee>(employeeForCreation);
        _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await _repository.SaveAsync();

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges) {

        var company = await _repository.Company
            .GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employeeForCompany = await _repository.Employee
            .GetEmployeeAsync(companyId, id, trackChanges);
        if (employeeForCompany is null)
            throw new EmployeeNotFoundException(id);

        _repository.Employee.DeleteEmployee(employeeForCompany);
        await _repository.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid cmpId, Guid id,
        EmployeeUpdateDto empUpdate, bool compTC, bool empTC) {

        var company = await _repository.Company.GetCompanyAsync(cmpId, compTC);
        if (company is null)
            throw new CompanyNotFoundException(cmpId);

        var empEntity = await _repository.Employee.GetEmployeeAsync(cmpId, id, empTC);
        if (empEntity is null)
            throw new EmployeeNotFoundException(id);

        _mapper.Map(empUpdate, empEntity);
        await _repository.SaveAsync();
    }

    public async Task<(EmployeeUpdateDto empToPatch, Employee empEntity)> GetEmployeeForPatchAsync
        (Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges) {

        var company = await _repository.Company.GetCompanyAsync(companyId, compTrackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var empEntity = await _repository.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);
        if (empEntity is null)
            throw new EmployeeNotFoundException(companyId);

        var empToPatch = _mapper.Map<EmployeeUpdateDto>(empEntity);

        return (empToPatch, empEntity);
    }
    public async Task SaveChangesForPatchAsync(EmployeeUpdateDto empToPatch, Employee empEntity) {

        _mapper.Map(empToPatch, empEntity);
        await _repository.SaveAsync();
    }

}

