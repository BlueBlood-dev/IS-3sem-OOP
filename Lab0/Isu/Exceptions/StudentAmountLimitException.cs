namespace Isu.Exceptions;

public class StudentAmountLimitException : IsuLogicException
{
    public StudentAmountLimitException(string message)
        : base(message)
    {
    }
}