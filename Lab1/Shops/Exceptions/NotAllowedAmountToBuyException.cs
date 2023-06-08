namespace Shops.Exceptions;

public class NotAllowedAmountToBuyException : ShopLogicException
{
    public NotAllowedAmountToBuyException(string message)
        : base(message)
    {
    }
}