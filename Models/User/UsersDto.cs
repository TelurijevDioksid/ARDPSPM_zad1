namespace Span.Culturio.Api.Models.User
{
    public class UsersDto
    {
        public IEnumerable<UserDto> Data { get; set; }
        public int TotalCount{ get; set; }
    }
}
