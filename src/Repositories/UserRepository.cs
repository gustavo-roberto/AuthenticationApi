using System;
using System.Linq;
using AuthenticationApi.Context;
using AuthenticationApi.Models;
using AuthenticationApi.Repositories.Dtos;

namespace AuthenticationApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthenticationContext _context;

        public UserRepository(AuthenticationContext context)
        {
            _context = context;             
        }

        public Guid Create(UserFormCreation user)
        {
            var id = Guid.NewGuid();
            var userDto = new UserDto(id, user.Email, user.Password);
            _context.UsersSecurity.Add(userDto);
            _context.SaveChanges();
            return id;
        }

        public void Delete(UserDto user)
        {
            _context.UsersSecurity.Remove(user);
            _context.SaveChanges();
        }

        public UserDto GetById(UserLogin user)
        {
           var userDto = _context.UsersSecurity.FirstOrDefault(x => x.Email == user.Email);

           return userDto;
        }
    }
}