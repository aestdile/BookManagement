using BookManagement.Domain.Common;
using BookManagement.Domain.Enums;

namespace BookManagement.Domain.Entities;

public class BorrowRecord : BaseEntity
{
    public int BookId { get; set; }
    public int UserId { get; set; }
    public DateTime BorrowedDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public BorrowStatus Status { get; set; } = BorrowStatus.Active;
    public int RenewalCount { get; set; } = 0;
    public decimal LateFee { get; set; } = 0;

    public Book Book { get; set; } = null!;
    public User User { get; set; } = null!;

    public bool IsOverdue() => !ReturnedDate.HasValue && DateTime.UtcNow > DueDate;

    public int GetOverdueDays()
    {
        if (!IsOverdue()) return 0;
        return (DateTime.UtcNow - DueDate).Days;
    }

    public decimal CalculateLateFee(decimal feePerDay)
    {
        var overdueDays = GetOverdueDays();
        return overdueDays > 0 ? overdueDays * feePerDay : 0;
    }
}