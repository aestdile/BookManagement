using BookManagement.Domain.Entities;

namespace BookManagement.Infrastructure.Repositories.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    Task<Book?> GetBookWithBorrowHistoryAsync(int bookId);
    Task<IEnumerable<Book>> GetAvailableBooksAsync();
    Task<(IEnumerable<Book> Books, int TotalCount)> GetPagedBooksAsync(
        int pageNumber, int pageSize, string? searchTerm, string? category);
    Task<Book?> GetByISBNAsync(string isbn);
}