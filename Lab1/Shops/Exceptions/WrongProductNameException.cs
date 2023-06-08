namespace Shops.Exceptions;

public class WrongProductNameException : ShopLogicException
{
    public WrongProductNameException(string message)
        : base(message)
    {
    }
}