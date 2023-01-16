using WebApiServer.DTOs;
using WebApiServer.Models;

namespace WebApiServer.Interfaces
{
    public interface IAuth
    {
        public Task<AuthResult> Login(LoginDto request);
        public Task<bool> Registration(RegistrationDto request);
    }
}
