using System;
using System.Security.Authentication;
using AuthenticationApi.Models;
using AuthenticationApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
            
        }

        [HttpPost("UserCreate")]
        public IActionResult AuthenticationUserCreate([FromBody] UserLogin user)
        {
            try
            {
                var idUser = _loginService.CreateLogin(user);
                return Ok(new {id = idUser});
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("UserLogin")]
        public IActionResult UserLogin([FromBody] UserLogin user)
        {
            try 
            {
                var token = _loginService.ValidateUser(user);
                
                return Ok(new {token = token});
            }
            catch(AuthenticationException ex)
            {
                return NotFound(ex.Message);
            }
            catch(InvalidOperationException)
            {
                return StatusCode(500);
            }
        }
    }
}