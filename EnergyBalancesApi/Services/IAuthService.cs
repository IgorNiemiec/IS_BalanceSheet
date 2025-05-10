using EnergyBalancesApi.Models;
public interface IAuthService
{
    Task<User> Register(UserRegisterDto dto);
    Task<string?> Login(UserLoginDto dto);
}
