using BookManagement.Application.DTOs.Books;
using BookManagement.Application.DTOs.Common;

namespace BookManagement.Application.Interfaces;

public interface IBookService
{
    Task<PagedResult<BookDto>> GetAllBooksAsync(int pageNumber, int pageSize, string? searchTerm, string? category);
    Task<BookDto> GetBookByIdAsync(int id);
    Task<BookStatusDto> GetBookStatusAsync(int id);
    Task<BookDto> CreateBookAsync(CreateBookDto createBookDto);
    Task<BookDto> UpdateBookAsync(int id, UpdateBookDto updateBookDto);
    Task DeleteBookAsync(int id);
}