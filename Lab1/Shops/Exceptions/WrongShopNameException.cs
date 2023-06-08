namespace Shops.Exceptions;

public class WrongShopNameException : ShopLogicException
{
    public WrongShopNameException(string message)
        : base(message)
    {
    }
}