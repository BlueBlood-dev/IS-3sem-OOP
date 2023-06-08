namespace Isu.Exceptions;

public class InvalidGroupNameLengthException : IsuLogicException
{
    public InvalidGroupNameLengthException(string message)
        : base(message)
    {
    }
}