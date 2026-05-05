using MUTZI.GUIA.APPLICATION.DTOs.User;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services
{
    public interface IAuthService
    {
        UserJwtDTO CurrentUser();
    }
}
