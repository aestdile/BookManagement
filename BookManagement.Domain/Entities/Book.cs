using BookManagement.Domain.Common;
using BookManagement.Domain.Enums;

namespace BookManagement.Domain.Entities;
public class Book : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
    public string? CoverImageUrl { get; set; }
    public BookStatus Status { get; set; } = BookStatus.Available;

    public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public bool IsAvailable() => AvailableCopies > 0 && Status == BookStatus.Available;

    public void CheckOut()
    {
        if (AvailableCopies <= 0)
            throw new InvalidOperationException("No copies available");

        AvailableCopies--;
        if (AvailableCopies == 0)
            Status = BookStatus.CheckedOut;
    }

    public void Return()
    {
        AvailableCopies++;
        if (AvailableCopies > 0)
            Status = BookStatus.Available;
    }
}
