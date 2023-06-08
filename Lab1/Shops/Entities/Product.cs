using Shops.Exceptions;

namespace Shops.Entities;

public class Product
{
    public Product(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new WrongProductNameException("Entered product name is invalid");
        Name = name;
        Id = Guid.NewGuid();
    }

    public string Name { get; }
    public Guid Id { get; }
}