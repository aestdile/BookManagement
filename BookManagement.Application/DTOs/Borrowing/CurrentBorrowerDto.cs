namespace BookManagement.Application.DTOs.Borrowing;

public class CurrentBorrowerDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime BorrowedDate { get; set; }
    public DateTime DueDate { get; set; }
    public int DaysRemaining { get; set; }
}
