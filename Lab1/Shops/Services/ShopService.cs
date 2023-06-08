using Shops.Entities;
using Shops.Exceptions;

namespace Shops.Services;

public class ShopService
{
    private const int AllowedAmountToBuy = 1;
    private readonly List<Product> _registeredProducts = new ();
    private readonly List<Shop> _shops = new ();

    public bool IsShopRegistered(Shop shop) => _shops.Any(s => s.Id == shop.Id);

    public bool IsProductRegistered(Product product) => _registeredProducts.Any(p => p.Id == product.Id);
    public bool AreProductsRegistered(IEnumerable<Product> products) => products.All(IsProductRegistered);
    public bool IsProductWithSuchNameExists(string name) => _registeredProducts.Any(p => p.Name == name);

    public Shop CreateShop(string name, string address)
    {
        var shop = new Shop(address, name);
        _shops.Add(shop);
        return shop;
    }

    public Product RegisterProduct(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (IsProductRegistered(product))
            throw new ProductAlreadyRegisteredException("such product was registered before");
        if (IsProductWithSuchNameExists(product.Name))
            throw new ProductWithSuchNameAlreadyExistsException("product with such name already exists");
        _registeredProducts.Add(product);
        return product;
    }

    public ProductInsideShop AddProductToShop(Product product, int price, int amount, Shop shop)
    {
        ArgumentNullException.ThrowIfNull(shop);
        ArgumentNullException.ThrowIfNull(product);
        if (!IsShopRegistered(shop))
            throw new ShopIsNotRegisteredException($"no such shop called {product.Name}");
        if (!IsProductRegistered(product))
            throw new ProductIsNotRegisteredException($"no such product called {product.Name}");
        var productInShop = new ProductInsideShop(product, price, amount);
        shop.AddProduct(productInShop);
        return productInShop;
    }

    public IReadOnlyCollection<ProductInsideShop> AddProductsToShop(List<ProductInsideShop> products, Shop shop)
    {
        ArgumentNullException.ThrowIfNull(shop);
        ArgumentNullException.ThrowIfNull(products);
        products.ForEach(p =>
        {
            if (!IsProductRegistered(p.Product))
                throw new ProductIsNotRegisteredException($"no such product called {p.Product.Name}");
        });
        shop.AddProducts(products);
        return products;
    }

    public void BuyProduct(Customer customer, Product product, int howMany, Shop shop)
    {
        ArgumentNullException.ThrowIfNull(customer);
        ArgumentNullException.ThrowIfNull(product);
        ArgumentNullException.ThrowIfNull(shop);
        if (howMany < AllowedAmountToBuy)
            throw new NotAllowedAmountToBuyException($"you are trying to buy less than {AllowedAmountToBuy} ");
        shop.BuyProduct(product, customer, howMany);
    }

    public Shop FindCheapestStore(List<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);
        var suitableShops = _shops.Where(s => s.CheckIfShopContainsProducts(products)).ToList();
        if (suitableShops.Count == 0)
            throw new NoSuitableShopsToPurchaseException("no shops containing such products remaining");
        Shop? shop = null;
        decimal totalPrice = decimal.MaxValue;
        foreach (Shop suitableShop in suitableShops)
        {
            decimal calculatedPrice =
                products.Sum(p => suitableShop
                    .Products
                    .Single(x => x.Product.Name == p.Name)
                    .Price);

            if (calculatedPrice >= totalPrice) continue;
            totalPrice = calculatedPrice;
            shop = suitableShop;
        }

        return shop!;
    }
}