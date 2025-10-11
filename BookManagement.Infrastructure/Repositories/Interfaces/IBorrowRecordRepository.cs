using BookManagement.Domain.Entities;
using BookManagement.Domain.Enums;

namespace BookManagement.Infrastructure.Repositories.Interfaces;

public interface IBorrowRecordRepository : IRepository<BorrowRecord>
{
    Task<IEnumerable<BorrowRecord>> GetActiveUserBorrowsAsync(int userId);
    Task<IEnumerable<BorrowRecord>> GetBookHistoryAsync(int bookId);
    Task<BorrowRecord?> GetActiveBorrowForBookAsync(int bookId, int userId);
    Task<IEnumerable<BorrowRecord>> GetOverdueBorrowsAsync();
    Task<int> GetActiveUserBorrowCountAsync(int userId);
}