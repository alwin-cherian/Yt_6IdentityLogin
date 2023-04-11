using Yt_6IdentityLogin.Models.DTO;

namespace Yt_6IdentityLogin.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);

        Task<Status> RegisterationAsync(RegisterationModel model);

        Task LogoutAsync();


    }
}
