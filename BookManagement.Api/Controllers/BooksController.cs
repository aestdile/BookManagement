using BookManagement.Application.DTOs.Books;
using BookManagement.Application.DTOs.Common;
using BookManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResult<BookDto>>>> GetAllBooks(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? category = null)
    {
        var result = await _bookService.GetAllBooksAsync(pageNumber, pageSize, searchTerm, category);
        return Ok(ApiResponse<PagedResult<BookDto>>.SuccessResponse(result));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BookDto>>> GetBookById(int id)
    {
        var result = await _bookService.GetBookByIdAsync(id);
        return Ok(ApiResponse<BookDto>.SuccessResponse(result));
    }

    [HttpGet("{id}/status")]
    public async Task<ActionResult<ApiResponse<BookStatusDto>>> GetBookStatus(int id)
    {
        var result = await _bookService.GetBookStatusAsync(id);
        return Ok(ApiResponse<BookStatusDto>.SuccessResponse(result));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<BookDto>>> CreateBook([FromBody] CreateBookDto createBookDto)
    {
        var result = await _bookService.CreateBookAsync(createBookDto);
        return CreatedAtAction(nameof(GetBookById), new { id = result.Id },
            ApiResponse<BookDto>.SuccessResponse(result, "Kitob qo'shildi"));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<BookDto>>> UpdateBook(int id, [FromBody] UpdateBookDto updateBookDto)
    {
        var result = await _bookService.UpdateBookAsync(id, updateBookDto);
        return Ok(ApiResponse<BookDto>.SuccessResponse(result, "Kitob yangilandi"));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteBook(int id)
    {
        await _bookService.DeleteBookAsync(id);
        return Ok(ApiResponse<object>.SuccessResponse(null, "Kitob o'chirildi"));
    }
}