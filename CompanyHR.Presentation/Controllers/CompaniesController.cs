using Entities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Shared.DataTransferObjects;

namespace CompanyHR.Presentation.Controllers;
[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase {
    private readonly IServiceManager _service;
    public CompaniesController(IServiceManager service) =>
        _service = service;

    [HttpGet]
    public IActionResult GetCompanies() {

        var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);

        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "CompanyById")]
    public IActionResult GetCompany(Guid id) {

        var company = _service.CompanyService.GetCompany(id, trackChanges: false);

        if (company is null)
            throw new CompanyNotFoundException(id);

        return Ok(company);
    }

    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyCreationDto company) {

        if (company is null)
            return BadRequest("CompanyForCreationDto object is null");

        var createdCompany = _service.CompanyService.CreateCompany(company);

        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id }, createdCompany);

    }

}
