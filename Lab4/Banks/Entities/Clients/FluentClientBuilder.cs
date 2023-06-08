using Banks.Exceptions;
using Banks.Models;
using Banks.Models.Passport;

namespace Banks.Entities.Clients;

public class FluentClientBuilder
{
    private string _name = string.Empty;
    private string _surname = string.Empty;
    private IPassport? _passport;
    private Address? _address;

    public FluentClientBuilder()
    {
    }

    public FluentClientBuilder SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("name should not be empty");
        _name = name;
        return this;
    }

    public FluentClientBuilder SetSurname(string surname)
    {
        if (string.IsNullOrWhiteSpace(surname))
            throw new ArgumentException("surname should not be empty");
        _surname = surname;
        return this;
    }

    public FluentClientBuilder SetPassport(IPassport passport)
    {
        ArgumentNullException.ThrowIfNull(passport);
        _passport = passport;
        return this;
    }

    public FluentClientBuilder SetAddress(Address address)
    {
        ArgumentNullException.ThrowIfNull(address);
        _address = address;
        return this;
    }

    public Client Build() => new Client(_surname, _name, _passport, _address);
}