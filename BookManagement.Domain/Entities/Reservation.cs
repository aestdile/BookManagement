using BookManagement.Domain.Common;

namespace BookManagement.Domain.Entities;

public class Reservation : BaseEntity
{
    public int BookId { get; set; }
    public int UserId { get; set; }
    public DateTime ReservedDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsNotified { get; set; } = false;

    public Book Book { get; set; } = null!;
    public User User { get; set; } = null!;
}