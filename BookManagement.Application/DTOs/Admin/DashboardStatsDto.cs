namespace BookManagement.Application.DTOs.Admin;

public class DashboardStatsDto
{
    public int TotalBooks { get; set; }
    public int TotalUsers { get; set; }
    public int ActiveBorrows { get; set; }
    public int OverdueBorrows { get; set; }
    public int AvailableBooks { get; set; }
    public decimal TotalLateFees { get; set; }
}