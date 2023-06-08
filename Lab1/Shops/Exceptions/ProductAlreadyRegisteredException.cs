namespace Shops.Exceptions;

public class ProductAlreadyRegisteredException : ShopLogicException
{
    public ProductAlreadyRegisteredException(string message)
        : base(message)
    {
    }
}