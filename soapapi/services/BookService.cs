using System.ServiceModel;
using SoapApi.Contracts;
using SoapApi.Dtos;
using SoapApi.Models;
using SoapApi.Repositories;

namespace SoapApi.Services;

public class BookService: IBookContract
{
    private readonly IBookRepository _bookRepository;
    
    public BookService(IBookRepository bookRepository){
        _bookRepository = bookRepository;
    }

    public async Task<IList<BookResponseDto>> GetBooksByName(string name, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetBooksByName(name, cancellationToken);
        return books.Select(book => book.ToDto()).ToList();
    }
}