using Notes_API.Models;
using Notes_API.Models.Dto;

namespace Notes_API.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<UserDTO> Register(RegisterationRequestDTO registrationRequestDTO);
    }
}
