namespace Shops.Exceptions;

public class NotEnoughMoneyException : ShopLogicException
{
    public NotEnoughMoneyException(string message)
        : base(message)
    {
    }
}