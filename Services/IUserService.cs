using Span.Culturio.Api.Models.Auth;
using Span.Culturio.Api.Models.User;

namespace Span.Culturio.Api.Services
{
    public interface IUserService
    {
        Task<UsersDto> GetUsersAsync(int pageSiz, int pageIdx);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> RegisterUser(RegisterUserDto register);
        Task<TokenDto> LoginUser(LoginDto loginDto);
    }
}
