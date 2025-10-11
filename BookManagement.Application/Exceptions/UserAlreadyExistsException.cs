namespace BookManagement.Application.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException()
        : base("Bu telefon raqam allaqachon ro'yxatdan o'tgan")
    {
    }

    public UserAlreadyExistsException(string message)
        : base(message)
    {
    }
}