using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using SoapApi.Infrastructure;
using SoapApi.Models;

namespace SoapApi.Repositories;

public class UserRepository: IUserRepository {
    private readonly RelationalDbContext _dbContext;

    public UserRepository(RelationalDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<UserModel> GetByIdAsync(Guid id, CancellationToken cancellationToken){
        var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return user.ToModel();
    }

    public async Task<IList<UserModel>> GetAllByEmailAsync(string email, CancellationToken cancellationToken){
        var users = await _dbContext.Users.Where(x => x.Email.Contains(email)).Select(x => x.ToModel()).ToListAsync(cancellationToken);
        return users;
    }

    public async Task<IList<UserModel>> GetAllAsync(CancellationToken cancellationToken){
        var users = await _dbContext.Users.Select(x => x.ToModel()).ToListAsync(cancellationToken);
        return users;
    }
}