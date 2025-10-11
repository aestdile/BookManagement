namespace BookManagement.Application.DTOs.Borrowing;

public class BorrowHistoryDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserPhoneNumber { get; set; } = string.Empty;
    public DateTime BorrowedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int RenewalCount { get; set; }
}