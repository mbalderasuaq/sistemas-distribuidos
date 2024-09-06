using SoapApi.Dtos;
using SoapApi.Infrastructure.Entities;
using SoapApi.Models;

public static class BookMapper
{
    public static BookModel ToModel(this BookEntity book)
    {
        return new BookModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Publisher = book.Publisher,
            PublishedDate = book.PublishedDate
        };
    }

    public static BookResponseDto ToDto(this BookModel book)
    {
        return new BookResponseDto
        {
            BookId = book.Id,
            Title = book.Title,
            Author = book.Author,
            Publisher = book.Publisher,
            PublishedDate = book.PublishedDate
        };
    }
}