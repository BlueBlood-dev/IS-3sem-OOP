namespace Isu.Exceptions;

public class StudentNotFoundException : IsuLogicException
{
    public StudentNotFoundException(string message)
        : base(message)
    {
    }
}