namespace Shops.Exceptions;

public class WrongProductPriceException : ShopLogicException
{
    public WrongProductPriceException(string message)
        : base(message)
    {
    }
}