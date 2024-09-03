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
}