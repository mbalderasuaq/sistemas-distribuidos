using RestApi.Exceptions;
using RestApi.Models;
using RestApi.Repositories;

namespace RestApi.Services;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;

    public GroupService(IGroupRepository groupRepository, IUserRepository userRepository)
    {
        _groupRepository = groupRepository;
        _userRepository = userRepository;
    }

    public async Task<GroupUserModel> GetGroupByIdAsync(string id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);

        if(group == null)
        {
            return null;
        }

        return new GroupUserModel{
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreatedAt,
            Users = (await Task.WhenAll(group.Users.Select(async user => await _userRepository.GetByIdAsync(user, cancellationToken)))).ToList()
        };
    }
    
    public async Task<IList<GroupUserModel>> GetAllByNameAsync(string name, int pageNumber, int pageSize, string orderBy, CancellationToken cancellationToken)
    {
        var groups = await _groupRepository.GetAllByNameAsync(name, pageNumber, pageSize, orderBy, cancellationToken);

        var groupUserModels = await Task.WhenAll(groups.Select(async group => new GroupUserModel{
            Id = group.Id,
            Name = group.Name,
            CreationDate = group.CreatedAt,
            Users = (await Task.WhenAll(group.Users.Select(async user => await _userRepository.GetByIdAsync(user, cancellationToken)))).ToList()
        }));

        return groupUserModels.ToList();
    }

    public async Task DeleteGroupByIdAsync(string id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);

        if(group is null)
        {
            throw new GroupNotFoundException();
        }

        await _groupRepository.DeleteByIdAsync(id, cancellationToken);
    }

    public async Task<GroupUserModel> CreateGroupAsync(string name, Guid[] users, CancellationToken cancellationToken)
    {
        if(users.Length == 0)
        {
            throw new InvalidGroupRequestFormatException();
        }

        var group = await _groupRepository.GetByNameAsync(name, cancellationToken);

        if(group != null)
        {
            throw new GroupAlreadyExistsException();
        }

        var newGroup = await _groupRepository.CreateAsync(name, users, cancellationToken);

        var usersList = await Task.WhenAll(newGroup.Users.Select(async user => await _userRepository.GetByIdAsync(user, cancellationToken)));

        if(usersList.Any(user => user is null))
        {
            throw new InvalidGroupUsersRequestException();
        }

        return new GroupUserModel{
            Id = newGroup.Id,
            Name = newGroup.Name,
            CreationDate = newGroup.CreatedAt,
            Users = usersList
        };
    }

    public async Task UpdateGroupAsync(string id, string name, Guid[] users, CancellationToken cancellationToken)
    {
        if(users.Length == 0)
        {
            throw new InvalidGroupRequestFormatException();
        }

        var group = await _groupRepository.GetByIdAsync(id, cancellationToken);

        if(group is null)
        {
            throw new GroupNotFoundException();
        }

        var groups = await _groupRepository.GetByNameAsync(name, cancellationToken);

        if(groups is not null && groups.Id != id){
            throw new GroupAlreadyExistsException();
        }

        var usersList = await Task.WhenAll(users.Select(async user => await _userRepository.GetByIdAsync(user, cancellationToken)));

        if(usersList.Any(user => user is null))
        {
            throw new InvalidGroupUsersRequestException();
        }

        await _groupRepository.UpdateAsync(id, name, users, cancellationToken);
    }
}