using AutoMapper;
using BookManagement.Application.DTOs.Books;
using BookManagement.Application.DTOs.Borrowing;
using BookManagement.Application.DTOs.Common;
using BookManagement.Application.Exceptions;
using BookManagement.Application.Interfaces;
using BookManagement.Domain.Entities;
using BookManagement.Domain.Enums;
using BookManagement.Infrastructure.Repositories.Interfaces;

namespace BookManagement.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly IMapper _mapper;

    public BookService(
        IBookRepository bookRepository,
        IBorrowRecordRepository borrowRecordRepository,
        IMapper mapper)
    {
        _bookRepository = bookRepository;
        _borrowRecordRepository = borrowRecordRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<BookDto>> GetAllBooksAsync(
        int pageNumber, int pageSize, string? searchTerm, string? category)
    {
        var (books, totalCount) = await _bookRepository.GetPagedBooksAsync(
            pageNumber, pageSize, searchTerm, category);

        var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books);

        return new PagedResult<BookDto>
        {
            Items = bookDtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<BookDto> GetBookByIdAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            throw new NotFoundException("Kitob", id);
        }

        return _mapper.Map<BookDto>(book);
    }

    public async Task<BookStatusDto> GetBookStatusAsync(int id)
    {
        var book = await _bookRepository.GetBookWithBorrowHistoryAsync(id);

        if (book == null)
        {
            throw new NotFoundException("Kitob", id);
        }

        var borrowHistory = _mapper.Map<List<BorrowHistoryDto>>(book.BorrowRecords);

        var activeBorrow = book.BorrowRecords
            .FirstOrDefault(br => br.Status == BorrowStatus.Active);

        CurrentBorrowerDto? currentBorrower = null;
        if (activeBorrow != null)
        {
            var daysRemaining = (activeBorrow.DueDate - DateTime.UtcNow).Days;
            currentBorrower = new CurrentBorrowerDto
            {
                UserId = activeBorrow.UserId,
                UserName = $"{activeBorrow.User.FirstName} {activeBorrow.User.LastName}",
                PhoneNumber = activeBorrow.User.PhoneNumber,
                BorrowedDate = activeBorrow.BorrowedDate,
                DueDate = activeBorrow.DueDate,
                DaysRemaining = daysRemaining > 0 ? daysRemaining : 0
            };
        }

        return new BookStatusDto
        {
            BookId = book.Id,
            Title = book.Title,
            Author = book.Author,
            Status = book.Status.ToString(),
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies,
            IsCurrentlyBorrowed = activeBorrow != null,
            CurrentBorrower = currentBorrower,
            BorrowHistory = borrowHistory.OrderByDescending(bh => bh.BorrowedDate).ToList(),
            TotalBorrows = book.BorrowRecords.Count
        };
    }

    public async Task<BookDto> CreateBookAsync(CreateBookDto createBookDto)
    {
        var existingBook = await _bookRepository.GetByISBNAsync(createBookDto.ISBN);
        if (existingBook != null)
        {
            throw new InvalidOperationException("Bu ISBN raqami allaqachon mavjud");
        }

        var book = _mapper.Map<Book>(createBookDto);
        book.Status = BookStatus.Available;
        book.CreatedAt = DateTime.UtcNow;

        await _bookRepository.AddAsync(book);
        await _bookRepository.SaveChangesAsync();

        return _mapper.Map<BookDto>(book);
    }

    public async Task<BookDto> UpdateBookAsync(int id, UpdateBookDto updateBookDto)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            throw new NotFoundException("Kitob", id);
        }

        book.Title = updateBookDto.Title;
        book.Author = updateBookDto.Author;
        book.Description = updateBookDto.Description;
        book.Category = updateBookDto.Category;
        book.PublicationYear = updateBookDto.PublicationYear;
        book.CoverImageUrl = updateBookDto.CoverImageUrl;

        var difference = updateBookDto.TotalCopies - book.TotalCopies;
        book.TotalCopies = updateBookDto.TotalCopies;
        book.AvailableCopies += difference;

        if (book.AvailableCopies < 0)
        {
            throw new InvalidOperationException("Nusxalar soni noto'g'ri");
        }

        if (book.AvailableCopies > 0)
        {
            book.Status = BookStatus.Available;
        }
        else
        {
            book.Status = BookStatus.CheckedOut;
        }

        book.UpdatedAt = DateTime.UtcNow;

        _bookRepository.Update(book);
        await _bookRepository.SaveChangesAsync();

        return _mapper.Map<BookDto>(book);
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = _bookRepository.GetByIdAsync(id).Result;

        if (book == null)
        {
            throw new NotFoundException("Kitob", id);
        }

        if (book.Status == BookStatus.CheckedOut)
        {
            throw new InvalidOperationException("Aktiv borrowga ega bo'lgan kitobni o'chirib bo'lmaydi");
        }

        _bookRepository.Remove(book);
        await _bookRepository.SaveChangesAsync();
    }
}