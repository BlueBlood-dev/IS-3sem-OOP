namespace Banks.Exceptions;

public class PassportException : Exception
{
    private PassportException(string message)
        : base(message)
    {
    }

    public static PassportException WrongSerialNumberException(string message)
    {
        return new PassportException(message);
    }

    public static PassportException WrongPassportNumberException(string message)
    {
        return new PassportException(message);
    }
}