using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Runtime.InteropServices;

namespace CompanyHR.Presentation.Controllers;
[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase {
    private readonly IServiceManager _service;
    public CompaniesController(IServiceManager service) => _service = service;

    [HttpGet]
    public IActionResult GetCompanies() {

        var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);

        return Ok(companies);
    }

}
