using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Data;
using BookManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Repositories.Implementations;

public class BookRepository : Repository<Book>, IBookRepository
{
    public BookRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Book?> GetBookWithBorrowHistoryAsync(int bookId)
    {   
        return await _dbSet
            .Include(b => b.BorrowRecords)
                .ThenInclude(br => br.User)
            .FirstOrDefaultAsync(b => b.Id == bookId);
    }

    public async Task<IEnumerable<Book>> GetAvailableBooksAsync()
    {
        return await _dbSet
            .Where(b => b.AvailableCopies > 0 && b.Status == Domain.Enums.BookStatus.Available)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Book> Books, int TotalCount)> GetPagedBooksAsync(
        int pageNumber, int pageSize, string? searchTerm, string? category)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            query = query.Where(b =>
                b.Title.ToLower().Contains(searchTerm) ||
                b.Author.ToLower().Contains(searchTerm) ||
                b.ISBN.Contains(searchTerm));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(b => b.Category.ToLower() == category.ToLower());
        }

        var totalCount = await query.CountAsync();

        var books = await query
            .OrderBy(b => b.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (books, totalCount);
    }

    public async Task<Book?> GetByISBNAsync(string isbn)
    {
        return await _dbSet.FirstOrDefaultAsync(b => b.ISBN == isbn);
    }
}