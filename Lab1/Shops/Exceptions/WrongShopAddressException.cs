namespace Shops.Exceptions;

public class WrongShopAddressException : ShopLogicException
{
    public WrongShopAddressException(string message)
        : base(message)
    {
    }
}