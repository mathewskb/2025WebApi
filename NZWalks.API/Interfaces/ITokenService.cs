using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(IdentityUser user, List<string> roles);
    }
}
