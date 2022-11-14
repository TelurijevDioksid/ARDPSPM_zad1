using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Span.Culturio.Api.Data;
using Span.Culturio.Api.Helpers;
using Span.Culturio.Api.Models.Auth;
using Span.Culturio.Api.Models.User;

namespace Span.Culturio.Api.Services.User
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(DataContext dataContext, IMapper mapper, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            this._configuration = configuration;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _dataContext.Users.FindAsync(id);
            if (user is null) return null;

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UsersDto> GetUsersAsync(int pageSiz, int pageIdx)
        {
            var users = await _dataContext.Users.Skip(pageSiz * pageIdx).Take(pageSiz).ToListAsync();

            var data = _mapper.Map<IEnumerable<UserDto>>(users);

            var usersDto = new UsersDto
            {
                Data = data,
                TotalCount = await _dataContext.Users.CountAsync()
            };

            return usersDto;
        }

        public async Task<UserDto> RegisterUser(RegisterUserDto register)
        {
            var user = _mapper.Map<Data.Entities.User>(register);

            UserHelper.CreatePasswordHash(register.Password, out byte[] passwordHash , out byte[] passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;
            
            _dataContext.Add(user);
            await _dataContext.SaveChangesAsync();
            var usdt = _mapper.Map<UserDto>(user);
            return usdt;
        }

        public async Task<TokenDto> LoginUser(LoginDto loginDto)
        {
            var user = _dataContext.Users.SingleOrDefault(o => o.UserName == loginDto.Username);

            if (user is null) 
                return null;

            if (!UserHelper.VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt)) return null;

            var token = UserHelper.CreateToken(loginDto, _configuration.GetSection("JWT_KEY").Value);

            return new TokenDto { Token = token };
        }
    }
}