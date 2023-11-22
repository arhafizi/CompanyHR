using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Services.Contracts;
using Shared.DataTransferObjects;

namespace Service;
internal sealed class CompanyService : ICompanyService {
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges) {

        var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges);

        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
        return companiesDto;
    }

    public async Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges) {

        var company = await _repository.Company.GetCompanyAsync(id, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(id);

        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public async Task<CompanyDto> CreateCompanyAsync(CompanyCreationDto company) {

        var companyEntity = _mapper.Map<Company>(company);

        _repository.Company.CreateCompany(companyEntity);
        await _repository.SaveAsync();

        var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
        return companyToReturn;
    }

    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trkChanges) {

        if (ids is null)
            throw new IdParametersBadRequestException();

        var cmpEntities = await _repository.Company.GetByIdsAsync(ids, trkChanges);
        if (ids.Count() != cmpEntities.Count())
            throw new CollectionByIdsBadRequestException();

        var cmpsToReturn = _mapper.Map<IEnumerable<CompanyDto>>(cmpEntities);
        return cmpsToReturn;
    }

    public async Task<(IEnumerable<CompanyDto> companies, string ids)>
        CreateCompanyCollectionAsync(IEnumerable<CompanyCreationDto> cmpCollection) {

        if (cmpCollection is null)
            throw new CompanyCollectionBadRequest();

        var cmpEntities = _mapper.Map<IEnumerable<Company>>(cmpCollection);

        foreach (var company in cmpEntities) {
            _repository.Company.CreateCompany(company);
        }

        await _repository.SaveAsync();

        var cmpCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(cmpEntities);

        var ids = string.Join(",", cmpCollectionToReturn.Select(c => c.Id));

        return (companies: cmpCollectionToReturn, ids: ids);
    }

    public async Task DeleteCompanyAsync(Guid cmpId, bool trkChanges) {
        
        var company = await _repository.Company.GetCompanyAsync(cmpId, trkChanges);
        if (company is null)
            throw new CompanyNotFoundException(cmpId);
       
        _repository.Company.DeleteCompany(company);
        await _repository.SaveAsync();
    }

    public async Task UpdateCompanyAsync(Guid cmpId, CompanyUpdateDto cmpForUpdate, bool trkChanges) {
        
        var cmpEntity = await _repository.Company.GetCompanyAsync(cmpId, trkChanges);
        if (cmpEntity is null)
            throw new CompanyNotFoundException(cmpId);
        
        _mapper.Map(cmpForUpdate, cmpEntity);
        await _repository.SaveAsync();
    }

}
