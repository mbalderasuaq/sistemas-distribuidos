using Microsoft.EntityFrameworkCore;
using SoapApi.Infrastructure;
using SoapApi.Models;

namespace SoapApi.Repositories;

public class BookRepository: IBookRepository
{
    private readonly RelationalDbContext _dbContext;

    public BookRepository(RelationalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IList<BookModel>> GetBooksByName(string name, CancellationToken cancellationToken){
        var books = await _dbContext.Books.AsNoTracking().Where(book => book.Title.Contains(name)).Select(book => book.ToModel()).ToListAsync(cancellationToken);
        return books;
    }
}