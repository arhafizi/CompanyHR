using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects;

namespace Services.Contracts;
public interface IAuthenticationService {
    Task<IdentityResult> RegisterUser(UserRegistrationDto userForRegistration);
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
    Task<TokenDto> CreateToken(bool populateExp);
    Task<TokenDto> RefreshToken(TokenDto tokenDto);

}