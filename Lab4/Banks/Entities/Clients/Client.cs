using System.Net;
using Banks.Models;
using Banks.Models.Passport;
using Banks.Observer;

namespace Banks.Entities.Clients;

public class Client : IObserver
{
    private readonly List<string> _notifications = new List<string>();

    public Client(string surname, string name, IPassport? passport, Address? address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("entered name is invalid");
        if (string.IsNullOrWhiteSpace(surname))
            throw new ArgumentException("entered surname is invalid");
        Surname = surname;
        Name = name;
        Id = Guid.NewGuid();
        Address = address;
        Passport = passport;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Surname { get; }
    public Address? Address { get; private set; }
    public IPassport? Passport { get; private set; }

    public IReadOnlyCollection<string> ClientNotifications => _notifications.AsReadOnly();

    public static FluentClientBuilder BuilderContext()
    {
        return new FluentClientBuilder();
    }

    public Guid ReturnObserverId() => Id;

    public void SetAddress(Address? address)
    {
        ArgumentNullException.ThrowIfNull(address);
        Address = address;
    }

    public void SetPassport(IPassport? passport)
    {
        ArgumentNullException.ThrowIfNull(passport);
        Passport = passport;
    }

    public void Update(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("notification message should be at least not null");
        _notifications.Add(message);
    }
}