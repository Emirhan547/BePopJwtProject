using BePopJwt.WebUI.Dtos.AuthDtos;

namespace BePopJwt.WebUI.Services.UserSessionServices
{
    public interface IUserSessionService
    {
        UserSessionViewModel GetCurrent();
        void SignIn(AuthResponseDto response);
        void UpdateDisplayName(string displayName);
        void SignOut();
    }
}
