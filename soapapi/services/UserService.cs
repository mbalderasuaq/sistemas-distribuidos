using System.ServiceModel;
using SoapApi.Contracts;
using SoapApi.Dtos;
using SoapApi.Repositories;

namespace SoapApi.Services;

public class UserService : IUserContract
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IList<UserResponseDto>> GetAll(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);

        if (users.Count > 0)
        {
            return users.Select(x => x.ToDto()).ToList();
        }

        throw new FaultException("No users found");
    }

    public async Task<IList<UserResponseDto>> GetAllByEmail(string email, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllByEmailAsync(email, cancellationToken);

        if (users.Count > 0)
        {
            return users.Select(x => x.ToDto()).ToList();
        }

        throw new FaultException("No users found");
    }

    public async Task<UserResponseDto> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (user is not null)
        {
            return user.ToDto();
        }

        throw new FaultException("User not found");
    }

    public async Task<bool> DeleteUserById(Guid userId, CancellationToken cancellationToken){
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if(user is null){
            throw new FaultException("User Not Found");
        }

        await _userRepository.DeleteByIdAsync(user, cancellationToken);

        return true;
    }

    public async Task<UserResponseDto> CreateUser(UserCreateRequestDto userRequest, CancellationToken cancellationToken)
    {
        var user = userRequest.ToModel();
        var createdUser = await _userRepository.CreateAsync(user, cancellationToken);
        return createdUser.ToDto();
    }

    public async Task<UserResponseDto> UpdateUser(UserUpdateRequestDto userRequest, CancellationToken cancellationToken)
    {
        var updatedUser = await _userRepository.UpdateAsync(userRequest.ToModel(), cancellationToken);
        return updatedUser.ToDto();
    }
}