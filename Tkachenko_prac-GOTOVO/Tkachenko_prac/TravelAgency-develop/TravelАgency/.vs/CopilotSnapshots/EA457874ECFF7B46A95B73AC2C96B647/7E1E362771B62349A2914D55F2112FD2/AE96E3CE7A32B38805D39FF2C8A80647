using TravelАgency.Domain.Response;
using TravelАgency.Domain.Models;
using System.Security.Claims;

namespace TravelAgency.Service.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResponse<string>> Register(User model);

        Task<BaseResponse<ClaimsIdentity>> Login(User model);

        Task<BaseResponse<ClaimsIdentity>> ConfirmEmail(User model, string code, string confirmCode);

        Task<BaseResponse<ClaimsIdentity>> IsCreatedAccount(User model);
    }
}
