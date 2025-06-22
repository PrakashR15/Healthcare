using static HospitalDTO;

namespace HospitalAPI.Interface
{
    public interface IUserService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto); 
        Task<(string token, string role)> LoginWithRoleAsync(LoginDto dto); 
    }


}
