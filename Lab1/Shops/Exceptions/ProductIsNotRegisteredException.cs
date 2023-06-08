namespace Shops.Exceptions;

public class ProductIsNotRegisteredException : ShopLogicException
{
    public ProductIsNotRegisteredException(string message)
        : base(message)
    {
    }
}