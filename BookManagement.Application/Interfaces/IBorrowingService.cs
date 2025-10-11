using BookManagement.Application.DTOs.Borrowing;

namespace BookManagement.Application.Interfaces;

public interface IBorrowingService
{
    Task<BorrowRecordDto> CheckoutBookAsync(int userId, int bookId);
    Task<BorrowRecordDto> ReturnBookAsync(int userId, int borrowRecordId);
    Task<BorrowRecordDto> RenewBookAsync(int userId, int borrowRecordId);
    Task<IEnumerable<BorrowRecordDto>> GetMyBooksAsync(int userId);
    Task<IEnumerable<BorrowRecordDto>> GetMyHistoryAsync(int userId);
    Task<IEnumerable<BorrowRecordDto>> GetOverdueBorrowsAsync();
}