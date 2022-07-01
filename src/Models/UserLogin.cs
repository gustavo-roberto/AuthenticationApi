using System;

namespace AuthenticationApi.Models
{
    public class UserLogin : UserFormCreation
    {
        public string Role { get; set; }   
    }
}