using BookManagement.Domain.Entities;
using BookManagement.Domain.Enums;
using BookManagement.Infrastructure.Data;
using BookManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Repositories.Implementations;

public class BorrowRecordRepository : Repository<BorrowRecord>, IBorrowRecordRepository
{
    public BorrowRecordRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BorrowRecord>> GetActiveUserBorrowsAsync(int userId)
    {
        return await _dbSet
            .Include(br => br.Book)
            .Where(br => br.UserId == userId && br.Status == BorrowStatus.Active)
            .OrderByDescending(br => br.BorrowedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<BorrowRecord>> GetBookHistoryAsync(int bookId)
    {
        return await _dbSet
            .Include(br => br.User)
            .Where(br => br.BookId == bookId)
            .OrderByDescending(br => br.BorrowedDate)
            .ToListAsync();
    }

    public async Task<BorrowRecord?> GetActiveBorrowForBookAsync(int bookId, int userId)
    {
        return await _dbSet
            .Include(br => br.Book)
            .FirstOrDefaultAsync(br =>
                br.BookId == bookId &&
                br.UserId == userId &&
                br.Status == BorrowStatus.Active);
    }

    public async Task<IEnumerable<BorrowRecord>> GetOverdueBorrowsAsync()
    {
        return await _dbSet
            .Include(br => br.Book)
            .Include(br => br.User)
            .Where(br =>
                br.Status == BorrowStatus.Active &&
                br.DueDate < DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<int> GetActiveUserBorrowCountAsync(int userId)
    {
        return await _dbSet.CountAsync(br =>
            br.UserId == userId &&
            br.Status == BorrowStatus.Active);
    }
}