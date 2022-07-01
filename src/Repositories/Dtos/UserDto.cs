
using System;

namespace AuthenticationApi.Repositories.Dtos
{
    public class UserDto
    {
        public UserDto(Guid id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}