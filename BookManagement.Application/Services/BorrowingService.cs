using AutoMapper;
using BookManagement.Application.DTOs.Borrowing;
using BookManagement.Application.Exceptions;
using BookManagement.Application.Interfaces;
using BookManagement.Domain.Entities;
using BookManagement.Domain.Enums;
using BookManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BookManagement.Application.Services;

public class BorrowingService : IBorrowingService
{
    private readonly IBookRepository _bookRepository;
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public BorrowingService(
        IBookRepository bookRepository,
        IBorrowRecordRepository borrowRecordRepository,
        IUserRepository userRepository,
        IMapper mapper,
        IConfiguration configuration)
    {
        _bookRepository = bookRepository;
        _borrowRecordRepository = borrowRecordRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<BorrowRecordDto> CheckoutBookAsync(int userId, int bookId)
    {
        var maxBooksPerUser = int.Parse(_configuration["BorrowingSettings:MaxBooksPerUser"] ?? "3");
        var borrowingPeriodDays = int.Parse(_configuration["BorrowingSettings:BorrowingPeriodDays"] ?? "14");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("Foydalanuvchi", userId);
        }

        var book = await _bookRepository.GetByIdAsync(bookId);
        if (book == null)
        {
            throw new NotFoundException("Kitob", bookId);
        }

        if (!book.IsAvailable())
        {
            throw new BookNotAvailableException("Kitob hozirda mavjud emas");
        }

        var activeBorrowCount = await _borrowRecordRepository.GetActiveUserBorrowCountAsync(userId);
        if (activeBorrowCount >= maxBooksPerUser)
        {
            throw new MaxBorrowLimitException(maxBooksPerUser);
        }

        var existingBorrow = await _borrowRecordRepository.GetActiveBorrowForBookAsync(bookId, userId);
        if (existingBorrow != null)
        {
            throw new InvalidOperationException("Siz bu kitobni allaqachon olgansiz");
        }

        var borrowRecord = new BorrowRecord
        {
            BookId = bookId,
            UserId = userId,
            BorrowedDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(borrowingPeriodDays),
            Status = BorrowStatus.Active,
            RenewalCount = 0,
            LateFee = 0,
            CreatedAt = DateTime.UtcNow
        };

        book.CheckOut();

        await _borrowRecordRepository.AddAsync(borrowRecord);
        _bookRepository.Update(book);
        await _borrowRecordRepository.SaveChangesAsync();

        borrowRecord.Book = book;
        borrowRecord.User = user;

        return _mapper.Map<BorrowRecordDto>(borrowRecord);
    }

    public async Task<BorrowRecordDto> ReturnBookAsync(int userId, int borrowRecordId)
    {
        var borrowRecord = await _borrowRecordRepository.GetByIdAsync(borrowRecordId);

        if (borrowRecord == null)
        {
            throw new NotFoundException("Borrow record", borrowRecordId);
        }

        if (borrowRecord.UserId != userId)
        {
            throw new InvalidOperationException("Bu kitob sizga tegishli emas");
        }

        if (borrowRecord.Status != BorrowStatus.Active)
        {
            throw new InvalidOperationException("Bu kitob allaqachon qaytarilgan");
        }

        var lateFeePerDay = decimal.Parse(_configuration["BorrowingSettings:LateFeePerDay"] ?? "1.5");
        borrowRecord.LateFee = borrowRecord.CalculateLateFee(lateFeePerDay);
        borrowRecord.ReturnedDate = DateTime.UtcNow;
        borrowRecord.Status = BorrowStatus.Returned;
        borrowRecord.UpdatedAt = DateTime.UtcNow;

        var book = await _bookRepository.GetByIdAsync(borrowRecord.BookId);
        if (book != null)
        {
            book.Return();
            _bookRepository.Update(book);
        }

        _borrowRecordRepository.Update(borrowRecord);
        await _borrowRecordRepository.SaveChangesAsync();

        var updatedRecord = await _borrowRecordRepository
            .FirstOrDefaultAsync(br => br.Id == borrowRecordId);

        return _mapper.Map<BorrowRecordDto>(updatedRecord);
    }

    public async Task<BorrowRecordDto> RenewBookAsync(int userId, int borrowRecordId)
    {
        var maxRenewals = int.Parse(_configuration["BorrowingSettings:MaxRenewals"] ?? "2");
        var borrowingPeriodDays = int.Parse(_configuration["BorrowingSettings:BorrowingPeriodDays"] ?? "14");

        var borrowRecord = await _borrowRecordRepository.GetByIdAsync(borrowRecordId);

        if (borrowRecord == null)
        {
            throw new NotFoundException("Borrow record", borrowRecordId);
        }

        if (borrowRecord.UserId != userId)
        {
            throw new InvalidOperationException("Bu kitob sizga tegishli emas");
        }

        if (borrowRecord.Status != BorrowStatus.Active)
        {
            throw new InvalidOperationException("Faqat aktiv borrowlarni yangilash mumkin");
        }

        if (borrowRecord.RenewalCount >= maxRenewals)
        {
            throw new InvalidOperationException($"Maksimal {maxRenewals} marta yangilash mumkin");
        }

        borrowRecord.DueDate = borrowRecord.DueDate.AddDays(borrowingPeriodDays);
        borrowRecord.RenewalCount++;
        borrowRecord.Status = BorrowStatus.Renewed;
        borrowRecord.UpdatedAt = DateTime.UtcNow;

        _borrowRecordRepository.Update(borrowRecord);
        await _borrowRecordRepository.SaveChangesAsync();

        var updatedRecord = await _borrowRecordRepository
            .FirstOrDefaultAsync(br => br.Id == borrowRecordId);

        return _mapper.Map<BorrowRecordDto>(updatedRecord);
    }

    public async Task<IEnumerable<BorrowRecordDto>> GetMyBooksAsync(int userId)
    {
        var borrowRecords = await _borrowRecordRepository.GetActiveUserBorrowsAsync(userId);
        return _mapper.Map<IEnumerable<BorrowRecordDto>>(borrowRecords);
    }

    public async Task<IEnumerable<BorrowRecordDto>> GetMyHistoryAsync(int userId)
    {
        var borrowRecords = await _borrowRecordRepository.FindAsync(br => br.UserId == userId);
        var ordered = borrowRecords.OrderByDescending(br => br.BorrowedDate);
        return _mapper.Map<IEnumerable<BorrowRecordDto>>(ordered);
    }

    public async Task<IEnumerable<BorrowRecordDto>> GetOverdueBorrowsAsync()
    {
        var overdueBorrows = await _borrowRecordRepository.GetOverdueBorrowsAsync();
        return _mapper.Map<IEnumerable<BorrowRecordDto>>(overdueBorrows);
    }
}
