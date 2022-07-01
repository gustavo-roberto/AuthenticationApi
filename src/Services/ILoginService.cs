using System;
using AuthenticationApi.Models;
using AuthenticationApi.Repositories.Dtos;

namespace AuthenticationApi.Services
{
    public interface ILoginService
    {
        public string ValidateUser(UserLogin user);
        public Guid CreateLogin(UserFormCreation user);
    }
}