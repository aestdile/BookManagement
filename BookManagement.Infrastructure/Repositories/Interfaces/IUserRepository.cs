using BookManagement.Domain.Entities;

namespace BookManagement.Infrastructure.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByPhoneNumberAsync(string phoneNumber);
    Task<User?> GetUserWithBorrowRecordsAsync(int userId);
    Task<bool> PhoneNumberExistsAsync(string phoneNumber);
}