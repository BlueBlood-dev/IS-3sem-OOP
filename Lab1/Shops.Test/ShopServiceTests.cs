using Shops.Entities;
using Shops.Exceptions;
using Shops.Services;
using Xunit;

namespace Shops.Test;

public class ShopServiceTests
{
    private readonly ShopService _service = new ShopService();

    [Fact]
    public void RegisterProduct_ProductIsRegistered()
    {
        var product = new Product("ak-47");
        _service.RegisterProduct(product);
        Assert.True(_service.IsProductRegistered(product));
    }

    [Fact]
    public void CreateShop_ShopIsRegistered()
    {
        Shop shop = _service.CreateShop("pink rabbit", "Kukushkina street");
        Assert.True(_service.IsShopRegistered(shop));
    }

    [Fact]
    public void CustomerBuysProduct_ThrowsNotEnoughMoneyException()
    {
        var customer = new Customer(1, "Barak Obama");
        var product = new Product("ak-47");
        _service.RegisterProduct(product);
        Shop shop = _service.CreateShop("pink rabbit", "Kukushkina street");
        var productToAdd = new ProductInsideShop(product, 1000, 100);
        shop.AddProduct(productToAdd);
        Assert.Throws<NotEnoughMoneyException>(() => _service.BuyProduct(customer, product, 2, shop));
    }

    [Fact]
    public void CustomerBuysProduct_ThrowsNotEnoughProductsInShopException()
    {
        var customer = new Customer(100000, "Barak Obama");
        var product = new Product("tec-9");
        _service.RegisterProduct(product);
        Shop shop = _service.CreateShop("pink rabbit", "Kukushkina street");
        var productToAdd = new ProductInsideShop(product, 1000, 1);
        shop.AddProduct(productToAdd);
        Assert.Throws<NotEnoughProductsInShopException>(() => _service.BuyProduct(customer, product, 3, shop));
    }

    [Fact]
    public void RegisterProduct_ThrowsProductWithSuchNameAlreadyExistsException()
    {
        var product = new Product("Chapman");
        _service.RegisterProduct(product);
        Assert.Throws<ProductWithSuchNameAlreadyExistsException>(() =>
            _service.RegisterProduct(new Product("Chapman")));
    }

    [Fact]
    public void BuyProduct_ShopGetsCustomerMoney()
    {
        decimal moneyBefore = 100;
        var customer = new Customer(moneyBefore, "Fredi Kats");
        var product = new Product("Clean architecture");
        _service.RegisterProduct(product);
        Shop shop = _service.CreateShop("Bukvoed", "Pionerskaya square");
        shop.AddProduct(new ProductInsideShop(product, 20, 3));
        _service.BuyProduct(customer, product, 1, shop);
        Assert.Equal(moneyBefore - shop.Money, customer.Money);
    }

    [Fact]
    public void FindCheapestStore_ReturnsCheapestStore()
    {
        var product1 = new Product("Milk");
        var product2 = new Product("Sugar");
        var fakeProduct = new Product("Cream soda");
        Shop shop1 = _service.CreateShop("FixPrice", "Nevsky prospect");
        _service.RegisterProduct(product1);
        _service.RegisterProduct(product2);
        _service.RegisterProduct(fakeProduct);
        _service.AddProductToShop(product1, 1, 10, shop1);
        _service.AddProductToShop(product2, 2, 100, shop1);
        Shop shop2 = _service.CreateShop("Pyaterochka", "Pioneskaya");
        _service.AddProductToShop(fakeProduct, 2, 100, shop2);
        Shop shop3 = _service.CreateShop("Skammer's", "Apraksin dvor");
        _service.AddProductToShop(product1, 1000, 10, shop3);
        _service.AddProductToShop(product2, 100, 10, shop3);
        _service.AddProductToShop(fakeProduct, 100, 10, shop3);
        Assert.Equal(shop1, _service.FindCheapestStore(new List<Product> { product1, product2 }));
    }
}