using SoapApi.Models;

namespace SoapApi.Repositories;

public interface IBookRepository {
    public Task<IList<BookModel>> GetBooksByName(string name, CancellationToken cancellationToken);
}