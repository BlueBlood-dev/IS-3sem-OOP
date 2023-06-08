namespace Isu.Exceptions;

public class InvalidGroupNameFormat : IsuLogicException
{
    public InvalidGroupNameFormat(string message)
        : base(message)
    {
    }
}