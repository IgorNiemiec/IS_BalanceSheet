using EnergyBalancesApi.Models;
using EnergyBalancesApi.Models.Dto;

public interface IAuthService
{
    Task<User> Register(UserRegisterDto dto);
    Task<string?> Login(UserLoginDto dto);
}
