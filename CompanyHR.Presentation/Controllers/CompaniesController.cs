﻿using CompanyHR.Presentation.ActionFilters;
using CompanyHR.Presentation.ModelBinders;
using Entities.Responses;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Shared.DataTransferObjects;

namespace CompanyHR.Presentation.Controllers;

[ApiVersion("1.0")]
[Route("api/companies")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[ResponseCache(CacheProfileName = "120SecondsDuration")]
public class CompaniesController : ControllerBase {
    private readonly IServiceManager _service;
    public CompaniesController(IServiceManager service) =>
        _service = service;

    /// <summary>
    /// Gets the list of all companies
    /// </summary>
    /// <returns>The companies list</returns>
    [HttpGet(Name = "GetCompanies")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetCompanies() {

        var companies = await _service.CompanyService.GetAllCompaniesAsync(false);

        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    // [ResponseCache(CacheProfileName = "120SecondsDuration")]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
    [HttpCacheValidation(MustRevalidate = false)]
    public async Task<IActionResult> GetCompany(Guid id) {

        var company = await _service.CompanyService.GetCompanyAsync(id, false);

        return Ok(company);
    }

    /// <summary>
    /// Creates a newly created company
    /// </summary>
    /// <param name="company"></param>
    /// <returns>A newly created company</returns>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="400">If the item is null</response>
    /// <response code="422">If the model is invalid</response>
    [HttpPost(Name = "CreateCompany")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(422)]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyCreationDto company) {

        var createdCompany = await _service.CompanyService.CreateCompanyAsync(company);

        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);
    }

    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public async Task<IActionResult> GetCompanyCollection(
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids) {

        var companies = await _service.CompanyService.GetByIdsAsync(ids, false);

        return Ok(companies);
    }

    [HttpPost("collection")]
    public async Task<IActionResult> CreateCompanyCollection(
        [FromBody] IEnumerable<CompanyCreationDto> companyCollection) {

        var result = await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection);

        return CreatedAtRoute("CompanyCollection", new { result.ids }, result.companies);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id) {

        await _service.CompanyService.DeleteCompanyAsync(id, false);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid id,
        [FromBody] CompanyUpdateDto company) {

        await _service.CompanyService.UpdateCompanyAsync(id, company, trackChanges: true);

        return NoContent();
    }

    [HttpOptions]
    public IActionResult GetCompaniesOptions() {

        Response.Headers.Add("Allow", "GET, OPTIONS, POST");
        return Ok();
    }
}
