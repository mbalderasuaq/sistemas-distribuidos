using System.ServiceModel;
using Microsoft.EntityFrameworkCore;
using SoapApi.Infrastructure;
using SoapApi.Models;

namespace SoapApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RelationalDbContext _dbContext;

    public UserRepository(RelationalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (user is null) return null;
        return user.ToModel();
    }

    public async Task<IList<UserModel>> GetAllByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users.AsNoTracking().Where(x => x.Email.Contains(email)).Select(x => x.ToModel()).ToListAsync(cancellationToken);
        return users;
    }

    public async Task<IList<UserModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await _dbContext.Users.AsNoTracking().Select(x => x.ToModel()).ToListAsync(cancellationToken);
        return users;
    }

    public async Task DeleteByIdAsync(UserModel user, CancellationToken cancellationToken)
    {
        var userEntity = user.ToEntity();
        _dbContext.Users.Remove(userEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserModel> CreateAsync(UserModel user, CancellationToken cancellationToken)
    {
        user.Id = Guid.NewGuid();
        await _dbContext.AddAsync(user.ToEntity(), cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<UserModel> UpdateAsync(UserModel user, CancellationToken cancellationToken)
    {
        var currentUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);

        if (currentUser is null) throw new FaultException("User not found");

        currentUser.FirstName = user.FirstName;
        currentUser.LastName = user.LastName;
        currentUser.Birthday = user.BirthDate;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return currentUser.ToModel();
    }
}