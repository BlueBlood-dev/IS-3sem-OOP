namespace Shops.Exceptions;

public class WrongAmountDecreaseValueException : ShopLogicException
{
    public WrongAmountDecreaseValueException(string message)
        : base(message)
    {
    }
}