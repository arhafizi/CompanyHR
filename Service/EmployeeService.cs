using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.LinkModels;
using Entities.Models;
using Services.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service;
public class EmployeeService : IEmployeeService {
    private readonly IRepositoryManager _repo;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    // private readonly IDataShaper<EmployeeDto> _dataShaper;
    private readonly IEmployeeLinks _employeeLinks;

    public EmployeeService(
        IRepositoryManager repository,
        ILoggerManager logger,
        IMapper mapper,
        IEmployeeLinks employeeLinks) {

        _repo = repository;
        _logger = logger;
        _mapper = mapper;
        _employeeLinks = employeeLinks;
    }

    public async Task<(LinkResponse linkResponse, MetaData metaData)> GetEmployeesAsync(
        Guid companyId, LinkParameters lp, bool trackChanges) {

        if (!lp.EmployeeParameters.ValidAgeRange)
            throw new MaxAgeRangeBadRequestException();

        await CheckIfCompanyExists(companyId, trackChanges);

        var employeesWithMetaData = await _repo.Employee
            .GetEmployeesAsync(companyId, lp.EmployeeParameters, trackChanges);

        var employeesDto = _mapper
            .Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);

        var links = _employeeLinks
            .TryGenerateLinks(employeesDto, lp.EmployeeParameters.Fields, companyId, lp.Context);

        return (linkResponse: links, metaData: employeesWithMetaData.MetaData);
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) {

        await CheckIfCompanyExists(companyId, trackChanges);

        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

        var employee = _mapper.Map<EmployeeDto>(employeeDb);

        return employee;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId,
        EmployeeCreationDto employeeForCreation, bool trackChanges) {

        await CheckIfCompanyExists(companyId, trackChanges);

        var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

        _repo.Employee.CreateEmployeeForCompany(companyId, employeeEntity);

        await _repo.SaveAsync();

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges) {

        await CheckIfCompanyExists(companyId, trackChanges);

        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, trackChanges);

        _repo.Employee.DeleteEmployee(employeeDb);

        await _repo.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id,
        EmployeeUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges) {

        await CheckIfCompanyExists(companyId, compTrackChanges);

        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

        _mapper.Map(employeeForUpdate, employeeDb);

        await _repo.SaveAsync();
    }

    public async Task<(EmployeeUpdateDto empToPatch, Employee empEntity)>
        GetEmployeeForPatchAsync(Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges) {

        await CheckIfCompanyExists(companyId, compTrackChanges);

        var employeeDb = await GetEmployeeForCompanyAndCheckIfItExists(companyId, id, empTrackChanges);

        var employeeToPatch = _mapper.Map<EmployeeUpdateDto>(employeeDb);

        return (empToPatch: employeeToPatch, empEntity: employeeDb);
    }

    public async Task SaveChangesForPatchAsync(EmployeeUpdateDto empToPatch, Employee empEntity) {

        _mapper.Map(empToPatch, empEntity);
        await _repo.SaveAsync();
    }
    private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges) {

        var company = await _repo.Company.GetCompanyAsync(companyId, trackChanges);

        if (company is null)
            throw new CompanyNotFoundException(companyId);
    }
    private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExists
        (Guid companyId, Guid id, bool trackChanges) {

        var employeeDb = await _repo.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if (employeeDb is null)
            throw new EmployeeNotFoundException(id);

        return employeeDb;
    }

}

