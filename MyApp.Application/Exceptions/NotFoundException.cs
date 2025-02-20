namespace MyApp.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message, int id) : base(message)
    {

    }
    public NotFoundException(string message) : base(message)
    {

    }
}
