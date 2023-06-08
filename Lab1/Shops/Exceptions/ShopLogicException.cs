namespace Shops.Exceptions;

public class ShopLogicException : Exception
{
    public ShopLogicException(string message)
        : base(message)
    {
    }
}