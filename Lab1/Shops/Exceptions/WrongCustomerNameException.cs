namespace Shops.Exceptions;

public class WrongCustomerNameException : ShopLogicException
{
    public WrongCustomerNameException(string message)
        : base(message)
    {
    }
}