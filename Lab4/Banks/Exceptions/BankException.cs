namespace Banks.Exceptions;

public class BankException : Exception
{
    private BankException(string message)
        : base(message)
    {
    }

    public static BankException BankAlreadyExistsException(string message)
    {
        return new BankException(message);
    }

    public static BankException BankNotExistException(string message)
    {
        return new BankException(message);
    }
}