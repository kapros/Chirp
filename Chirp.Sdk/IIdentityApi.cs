using Chirp.Contracts.V1;
using Chirp.Contracts.V1.Requests;
using Chirp.Contracts.V1.Responses;
using Refit;
using System.Threading.Tasks;

namespace Chirp.Sdk
{
    public interface IIdentityApi
    {
        [Post("/" + ApiRoutes.Identity.Register)]
        Task<ApiResponse<AuthSuccessResponse>> RegisterAsync([Body] UserRegistrationRequest request);


        [Post("/" + ApiRoutes.Identity.Login)]
        Task<ApiResponse<AuthSuccessResponse>> LoginAsync([Body] UserLoginRequest request);


        [Post("/" + ApiRoutes.Identity.Refresh)]
        Task<ApiResponse<AuthSuccessResponse>> RefreshAsync([Body] RefreshTokenRequest request);
    }
}
