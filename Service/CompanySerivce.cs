using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Services.Contracts;
using Shared.DataTransferObjects;

namespace Service;
internal sealed class CompanyService : ICompanyService {
    private readonly IRepositoryManager _repo;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper) {
        _repo = repository;
        _logger = logger;
        _mapper = mapper;
    }

    private async Task<Company> GetCompanyAndCheckIfItExists(Guid id, bool trackChanges) {

        var company = await _repo.Company.GetCompanyAsync(id, trackChanges);

        if (company is null)
            throw new CompanyNotFoundException(id);

        return company;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges) {

        var companies = await _repo.Company.GetAllCompaniesAsync(trackChanges);

        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

        return companiesDto;
    }

    public async Task<CompanyDto> GetCompanyAsync(Guid id, bool trackChanges) {
      
        var company = await GetCompanyAndCheckIfItExists(id, trackChanges);

        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public async Task<CompanyDto> CreateCompanyAsync(CompanyCreationDto company) {

        var companyEntity = _mapper.Map<Company>(company);

        _repo.Company.CreateCompany(companyEntity);
        await _repo.SaveAsync();

        var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);
        return companyToReturn;
    }

    public async Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trkChanges) {

        if (ids is null)
            throw new IdParametersBadRequestException();

        var cmpEntities = await _repo.Company.GetByIdsAsync(ids, trkChanges);
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
            _repo.Company.CreateCompany(company);
        }

        await _repo.SaveAsync();

        var cmpCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(cmpEntities);

        var ids = string.Join(",", cmpCollectionToReturn.Select(c => c.Id));

        return (companies: cmpCollectionToReturn, ids: ids);
    }

    public async Task DeleteCompanyAsync(Guid cmpId, bool trkChanges) {

        var company = await GetCompanyAndCheckIfItExists(cmpId, trkChanges);

        _repo.Company.DeleteCompany(company);
        await _repo.SaveAsync();
    }

    public async Task UpdateCompanyAsync(Guid cmpId, CompanyUpdateDto cmpForUpdate, bool trkChanges) {

        var company = await GetCompanyAndCheckIfItExists(cmpId, trkChanges);

        _mapper.Map(cmpForUpdate, company);
        await _repo.SaveAsync();
    }

}
