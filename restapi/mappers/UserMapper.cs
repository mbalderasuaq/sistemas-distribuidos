using RestApi.Dtos;
using RestApi.Infrastructure.Soap;
using RestApi.Models;

namespace RestApi.Mappers;

public static class UserMapper
{
    public static UserModel ToDomain(this UserResponseDto user)
    {
        if(user == null)
        {
            return null;
        }

        return new UserModel
        {
            Id = user.UserId,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.BirthDate
        };
    }

    public static UserResponse ToDto(this UserModel user)
    {
        if(user == null)
        {
            return null;
        }

        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.FirstName + " " + user.LastName,
            BirthDate = user.BirthDate
        };
    }

    public static List<UserResponse> ToDto(this IList<UserModel> users)
    {
        if(users == null)
        {
            return null;
        }

        return users.Select(user => user.ToDto()).ToList();
    }
}