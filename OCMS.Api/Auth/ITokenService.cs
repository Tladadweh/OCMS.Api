using OCMS.Domain.Models;

namespace OCMS.Api.Auth
{
    public interface ITokenService
    {
        string Generate(User user);

    }
}
