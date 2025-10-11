using BookManagement.Domain.Entities;
using BookManagement.Infrastructure.Data;
using BookManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Repositories.Implementations;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByPhoneNumberAsync(string phoneNumber)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
    }

    public async Task<User?> GetUserWithBorrowRecordsAsync(int userId)
    {
        return await _dbSet
            .Include(u => u.BorrowRecords)
                .ThenInclude(br => br.Book)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
    {
        return await _dbSet.AnyAsync(u => u.PhoneNumber == phoneNumber);
    }
}