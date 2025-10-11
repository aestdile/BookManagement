using BookManagement.Application.DTOs.Admin;
using BookManagement.Application.DTOs.Borrowing;
using BookManagement.Application.DTOs.Common;
using BookManagement.Application.Interfaces;
using BookManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IBookRepository _bookRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBorrowRecordRepository _borrowRecordRepository;
    private readonly IBorrowingService _borrowingService;

    public AdminController(
        IBookRepository bookRepository,
        IUserRepository userRepository,
        IBorrowRecordRepository borrowRecordRepository,
        IBorrowingService borrowingService)
    {
        _bookRepository = bookRepository;
        _userRepository = userRepository;
        _borrowRecordRepository = borrowRecordRepository;
        _borrowingService = borrowingService;
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<ApiResponse<DashboardStatsDto>>> GetDashboardStats()
    {
        var totalBooks = await _bookRepository.CountAsync();
        var totalUsers = await _userRepository.CountAsync();
        var activeBorrows = await _borrowRecordRepository.CountAsync(br =>
            br.Status == Domain.Enums.BorrowStatus.Active);
        var availableBooks = (await _bookRepository.GetAvailableBooksAsync()).Count();
        var overdueBorrows = (await _borrowingService.GetOverdueBorrowsAsync()).Count();

        var stats = new DashboardStatsDto
        {
            TotalBooks = totalBooks,
            TotalUsers = totalUsers,
            ActiveBorrows = activeBorrows,
            AvailableBooks = availableBooks,
            OverdueBorrows = overdueBorrows,
            TotalLateFees = 0 // Can be calculated if needed
        };

        return Ok(ApiResponse<DashboardStatsDto>.SuccessResponse(stats));
    }

    [HttpGet("borrows/overdue")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BorrowRecordDto>>>> GetOverdueBorrows()
    {
        var result = await _borrowingService.GetOverdueBorrowsAsync();
        return Ok(ApiResponse<IEnumerable<BorrowRecordDto>>.SuccessResponse(result));
    }
}