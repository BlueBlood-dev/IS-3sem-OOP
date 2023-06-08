using Shops.Exceptions;

namespace Shops.Entities;

public class Customer
{
    private const int AllowedAmountOfMoney = 0;

    public Customer(decimal money, string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        if (string.IsNullOrWhiteSpace(name))
            throw new WrongCustomerNameException("Entered name is invalid");
        if (money < AllowedAmountOfMoney)
            throw new WrongCustomerMoneyAmountException($"Amount of money must be bigger than {AllowedAmountOfMoney}");
        Money = money;
        Name = name;
    }

    public string Name { get; }
    public decimal Money { get; private set; }

    public void DecreaseMoneyAfterPurchase(decimal spentMoney)
    {
        if (Money - spentMoney < AllowedAmountOfMoney || spentMoney <= AllowedAmountOfMoney)
            throw new WrongCustomerMoneyAmountException("can't set such amount of money");
        Money -= spentMoney;
    }
}