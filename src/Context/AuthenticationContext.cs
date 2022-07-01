using Microsoft.EntityFrameworkCore;
using AuthenticationApi.Repositories.Dtos;

namespace AuthenticationApi.Context
{
    public class AuthenticationContext : DbContext
    {
        public DbSet<UserDto> UsersSecurity {get; set;}

        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { }
    }
}