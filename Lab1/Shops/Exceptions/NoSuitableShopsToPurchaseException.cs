namespace Shops.Exceptions;

public class NoSuitableShopsToPurchaseException : ShopLogicException
{
    public NoSuitableShopsToPurchaseException(string message)
        : base(message)
    {
    }
}