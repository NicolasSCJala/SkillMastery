using MapsterMapper;
using SkillMasteryAPI.Application.DTOs.User;
using SkillMasteryAPI.Application.Services.Interfaces;
using SkillMasteryAPI.Infrastructure.Repositories.Interfaces;
using SkillMasteryAPI.Domain.Models;
using SkillMasteryAPI.Application.CrossCutting;

namespace SkillMasteryAPI.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return _mapper.Map<IEnumerable<UserDTO>>(users);
    }

    public async Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDTO)
    {
        var user = _mapper.Map<User>(createUserDTO);
        var createdUser = await _userRepository.CreateUserAsync(user);
        return _mapper.Map<UserDTO>(createdUser);
    }

    async public Task<UserDTO?> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        NotFoundException.ThrowIfNull(user, $"User with id {id} not found");


        var deletedUser = await _userRepository.DeleteUserAsync(user!);

        return _mapper.Map<UserDTO>(deletedUser);
    }

    public async Task<UserDTO> EditUserAsync(UserDTO editUserDTO)
    {
        var user = _mapper.Map<User>(editUserDTO);
        var editedUser = await _userRepository.EditUserAsync(user);
        return _mapper.Map<UserDTO>(editedUser);
    }
}
