namespace BookManagement.Application.Exceptions;

public class BookNotAvailableException : Exception
{
    public BookNotAvailableException()
        : base("Kitob hozirda mavjud emas")
    {
    }

    public BookNotAvailableException(string message)
        : base(message)
    {
    }
}