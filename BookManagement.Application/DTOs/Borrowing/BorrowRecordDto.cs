using BookManagement.Application.DTOs.Books;

namespace BookManagement.Application.DTOs.Borrowing;

public class BorrowRecordDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string BookAuthor { get; set; } = string.Empty;
    public DateTime BorrowedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int RenewalCount { get; set; }
    public decimal LateFee { get; set; }
    public bool IsOverdue { get; set; }
    public int? OverdueDays { get; set; }
}