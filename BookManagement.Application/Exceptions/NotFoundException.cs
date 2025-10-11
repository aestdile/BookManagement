namespace BookManagement.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, int id)
        : base($"{entityName} with ID {id} topilmadi")
    {
    }

    public NotFoundException(string message)
        : base(message)
    {
    }
}