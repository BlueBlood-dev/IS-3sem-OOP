namespace Shops.Exceptions;

public class ShopDoesntContainSuchProduct : ShopLogicException
{
    public ShopDoesntContainSuchProduct(string message)
        : base(message)
    {
    }
}