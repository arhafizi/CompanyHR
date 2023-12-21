using CompanyHR.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Shared.DataTransferObjects;

namespace CompanyHR.Presentation.Controllers;
[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase {
    private readonly IServiceManager _service;
    public AuthenticationController(IServiceManager service) => _service = service;

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userRegDto) {

        var result =
            await _service.AuthenticationService.RegisterUser(userRegDto);

        if (!result.Succeeded) {
            foreach (var error in result.Errors) {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }

    [HttpPost("login")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user) {

        if (!await _service.AuthenticationService.ValidateUser(user))
            return Unauthorized();

        var tokenDto = await _service.AuthenticationService
            .CreateToken(populateExp: true);
        
        return Ok(tokenDto);
    }
}