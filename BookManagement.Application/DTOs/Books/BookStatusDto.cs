using BookManagement.Application.DTOs.Borrowing;

namespace BookManagement.Application.DTOs.Books;

public class BookStatusDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public bool IsCurrentlyBorrowed { get; set; }
    public CurrentBorrowerDto? CurrentBorrower { get; set; }
    public List<BorrowHistoryDto> BorrowHistory { get; set; } = new();
    public int TotalBorrows { get; set; }
}