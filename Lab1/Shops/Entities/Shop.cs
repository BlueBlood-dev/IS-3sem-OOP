using Shops.Exceptions;

namespace Shops.Entities;

public class Shop
{
    private readonly List<ProductInsideShop> _products = new List<ProductInsideShop>();

    public Shop(string address, string name)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new WrongShopAddressException("Entered address is invalid");
        if (string.IsNullOrWhiteSpace(name))
            throw new WrongShopNameException("Entered name is invalid");
        Address = address;
        Name = name;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Address { get; }
    public decimal Money { get; private set; } = 0;

    public IReadOnlyCollection<ProductInsideShop> Products => _products.AsReadOnly();

    public void AddProduct(ProductInsideShop product)
    {
        ArgumentNullException.ThrowIfNull(product);
        _products.Add(product);
    }

    public void AddProducts(List<ProductInsideShop> productInsideShops)
    {
        productInsideShops.ForEach(AddProduct);
    }

    public void BuyProduct(Product product, Customer customer, int howMany)
    {
        ProductInsideShop? foundProduct = FindProductInsideShop(product);
        if (foundProduct is null)
            throw new ShopDoesntContainSuchProduct($"{Name} doesn't have any {product.Name}");
        decimal purchaseCost = howMany * foundProduct.Price;
        if (customer.Money < purchaseCost)
            throw new NotEnoughMoneyException($"{customer.Name} is too poor to afford {product.Name}");
        if (foundProduct.Amount < howMany)
        {
            throw new NotEnoughProductsInShopException(
                $"you want to buy {howMany} of {product.Name}, but only {foundProduct.Amount} provided");
        }

        foundProduct.DecreaseAmount(howMany);
        customer.DecreaseMoneyAfterPurchase(purchaseCost);
        Money += purchaseCost;
    }

    public void ChangePrice(Product product, int newPrice)
    {
        ProductInsideShop? foundProduct = FindProductInsideShop(product);
        if (foundProduct is null)
            throw new ShopDoesntContainSuchProduct($"{Name} doesn't have any {product.Name}");
        foundProduct.SetPrice(newPrice);
    }

    public bool CheckIfShopContainsProducts(IReadOnlyList<Product> products) =>
        products.All(p => _products.Any(productInShop => productInShop.Product.Name == p.Name));

    private ProductInsideShop? FindProductInsideShop(Product product) =>
        _products.SingleOrDefault(p => p.Product.Id == product.Id);
}