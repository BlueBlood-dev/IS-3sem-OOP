namespace Shops.Exceptions;

public class WrongCustomerMoneyAmountException : ShopLogicException
{
    public WrongCustomerMoneyAmountException(string message)
        : base(message)
    {
    }
}