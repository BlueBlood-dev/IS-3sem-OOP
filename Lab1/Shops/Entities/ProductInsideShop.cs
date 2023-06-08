using Shops.Exceptions;

namespace Shops.Entities;

public class ProductInsideShop
{
    private const int MinimalRequiredPrice = 1;
    private const int MinimalAmount = 0;
    public ProductInsideShop(Product product, decimal price, int amount)
    {
        ArgumentNullException.ThrowIfNull(product);
        ArgumentNullException.ThrowIfNull(price);
        ArgumentNullException.ThrowIfNull(amount);
        if (price < MinimalRequiredPrice)
        {
            throw new WrongProductPriceException(
                $"price should be at least bigger than {MinimalRequiredPrice}, entered price is {price}");
        }

        Product = product;
        Price = price;
        Amount = amount;
    }

    public decimal Price { get; private set; }
    public int Amount { get; private set; }

    public Product Product { get; }

    public void SetPrice(decimal newPrice)
    {
        if (newPrice < MinimalRequiredPrice)
            throw new WrongProductPriceException("you are trying to set invalid price value");
        Price = newPrice;
    }

    public void DecreaseAmount(int howMany)
    {
        if (Amount - howMany < MinimalAmount)
            throw new WrongAmountDecreaseValueException("can't set amount to such number");
        Amount -= howMany;
    }
}