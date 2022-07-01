using System;
using AuthenticationApi.Models;
using AuthenticationApi.Repositories.Dtos;

namespace AuthenticationApi.Repositories
{
    public interface IUserRepository
    {
        public UserDto GetById(UserLogin user);
        public Guid Create(UserFormCreation user);
        public void Delete(UserDto user);
    }
}