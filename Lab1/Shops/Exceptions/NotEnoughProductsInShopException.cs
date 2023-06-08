namespace Shops.Exceptions;

public class NotEnoughProductsInShopException : ShopLogicException
{
    public NotEnoughProductsInShopException(string message)
        : base(message)
    {
    }
}