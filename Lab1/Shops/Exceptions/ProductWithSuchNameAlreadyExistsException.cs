namespace Shops.Exceptions;

public class ProductWithSuchNameAlreadyExistsException : ShopLogicException
{
    public ProductWithSuchNameAlreadyExistsException(string message)
        : base(message)
    {
    }
}