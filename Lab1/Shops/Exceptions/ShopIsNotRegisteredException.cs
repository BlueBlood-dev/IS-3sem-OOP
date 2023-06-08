namespace Shops.Exceptions;

public class ShopIsNotRegisteredException : ShopLogicException
{
    public ShopIsNotRegisteredException(string message)
        : base(message)
    {
    }
}