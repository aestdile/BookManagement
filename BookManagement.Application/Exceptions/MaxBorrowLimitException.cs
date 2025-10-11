namespace BookManagement.Application.Exceptions;

public class MaxBorrowLimitException : Exception
{
    public MaxBorrowLimitException(int maxLimit)
        : base($"Siz maksimal {maxLimit} ta kitob olishingiz mumkin")
    {
    }

    public MaxBorrowLimitException(string message)
        : base(message)
    {
    }
}