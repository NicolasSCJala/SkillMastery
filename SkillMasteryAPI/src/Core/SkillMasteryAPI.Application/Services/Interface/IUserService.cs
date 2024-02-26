using SkillMasteryAPI.Application.DTOs.User;

namespace SkillMasteryAPI.Application.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDTO);
    Task<UserDTO?> DeleteUserAsync(int id);
    Task<UserDTO> EditUserAsync(UserDTO goalDTO);

}
