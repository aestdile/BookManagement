using BookManagement.Application.DTOs.Borrowing;
using BookManagement.Application.DTOs.Common;
using BookManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BorrowingController : ControllerBase
{
    private readonly IBorrowingService _borrowingService;

    public BorrowingController(IBorrowingService borrowingService)
    {
        _borrowingService = borrowingService;
    }

    [HttpPost("checkout/{bookId}")]
    public async Task<ActionResult<ApiResponse<BorrowRecordDto>>> CheckoutBook(int bookId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _borrowingService.CheckoutBookAsync(userId, bookId);
        return Ok(ApiResponse<BorrowRecordDto>.SuccessResponse(result, "Kitob muvaffaqiyatli olindi"));
    }

    [HttpPost("return/{borrowRecordId}")]
    public async Task<ActionResult<ApiResponse<BorrowRecordDto>>> ReturnBook(int borrowRecordId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _borrowingService.ReturnBookAsync(userId, borrowRecordId);
        return Ok(ApiResponse<BorrowRecordDto>.SuccessResponse(result, "Kitob qaytarildi"));
    }

    [HttpPost("renew/{borrowRecordId}")]
    public async Task<ActionResult<ApiResponse<BorrowRecordDto>>> RenewBook(int borrowRecordId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _borrowingService.RenewBookAsync(userId, borrowRecordId);
        return Ok(ApiResponse<BorrowRecordDto>.SuccessResponse(result, "Kitob muddati uzaytirildi"));
    }

    [HttpGet("my-books")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BorrowRecordDto>>>> GetMyBooks()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _borrowingService.GetMyBooksAsync(userId);
        return Ok(ApiResponse<IEnumerable<BorrowRecordDto>>.SuccessResponse(result));
    }

    [HttpGet("history")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BorrowRecordDto>>>> GetMyHistory()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _borrowingService.GetMyHistoryAsync(userId);
        return Ok(ApiResponse<IEnumerable<BorrowRecordDto>>.SuccessResponse(result));
    }
}