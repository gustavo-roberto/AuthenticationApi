using AuthenticationApi.Models;
using Microsoft.AspNetCore.Identity;
using AuthenticationApi.Repositories;
using System.Security.Authentication;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using AuthenticationApi.Repositories.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace AuthenticationApi.Services
{
    public class LoginService : ILoginService
    {
        private readonly IMemoryCache _cache;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public LoginService(IUserRepository userRepository, IConfiguration configuration, IMemoryCache cache)
        {
            _cache = cache;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public Guid CreateLogin(UserFormCreation user)
        {
            ValidateUserForCreation(user);
            PasswordToHash(user);            
            var id = _userRepository.Create(user);
            return id;
        }

        private void PasswordToHash(UserFormCreation user)
        {
            var passwordHasher = new PasswordHasher<UserFormCreation>();
            user.Password = passwordHasher.HashPassword(user, user.Password);
        }

        private void ValidateUserForCreation(UserFormCreation user)
        {
            //implementar validações
        }

        public string ValidateUser(UserLogin user)
        {
            var consultedUser = _userRepository.GetById(user);
            
            if(consultedUser == null)
            {
                throw new AuthenticationException("Incorrect Email.");
            }
            
            validateHash(user, consultedUser);
            string token = generateJwtToken(user);
            user.Password = "";
            return token;
        }

        private string generateJwtToken(UserLogin user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["HashCode:HashTokenKey"];
            var key = Encoding.ASCII.GetBytes(secretKey); //mudar depois e colocar em settings
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity( new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private void validateHash(UserLogin user, UserDto consultedUser)
        {
            var passwordHasher = new PasswordHasher<UserLogin>();
            var status = passwordHasher.VerifyHashedPassword(user, consultedUser.Password, user.Password);
            
            switch(status)
            {
                case PasswordVerificationResult.Failed:
                    verifyCache(consultedUser);
                    throw new AuthenticationException("Incorrect password.");
                case PasswordVerificationResult.Success:
                    CleanCache();
                    return;
                default:
                    throw new InvalidOperationException("Password validation processing error.");
            }
        }

        private void CleanCache()
        {
            _cache.Remove("failedAttempts");

        }
        private void verifyCache(UserDto user)
        {
            var cacheDataSet = _cache.Get<CacheDataSet>("failedAttempts");

            if(cacheDataSet == null)
            {
                var expiresAt = DateTimeOffset.Now.AddDays(1);
                var newCacheDataSet = new CacheDataSet(){ExpiresAt = expiresAt, attempts = 1};

                _cache.Set("failedAttempts", newCacheDataSet, expiresAt);
            }
            else
            {
                if (cacheDataSet.attempts > 3)
                {
                    _userRepository.Delete(user);
                    throw new AuthenticationException("Credencials were deleted. Please, create a new user.");
                }
                else
                {
                    cacheDataSet.attempts++;
                    _cache.Set("failedAttempts", cacheDataSet, cacheDataSet.ExpiresAt);
                }
            }
        }
    }
}