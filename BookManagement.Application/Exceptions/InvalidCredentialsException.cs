namespace BookManagement.Application.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException()
        : base("Telefon raqam yoki parol noto'g'ri")
    {
    }

    public InvalidCredentialsException(string message)
        : base(message)
    {
    }
}