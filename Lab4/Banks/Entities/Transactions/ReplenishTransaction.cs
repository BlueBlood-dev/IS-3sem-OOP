using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Exceptions;

namespace Banks.Entities.Transactions;

public class ReplenishTransaction : ITransaction
{
    private readonly Bank _bank;
    private readonly Guid _clientId;
    private readonly Guid _destinationAccountId;
    private readonly decimal _money;

    public ReplenishTransaction(Guid destinationAccountId, decimal money, Bank bank, Guid clientId)
    {
        ArgumentNullException.ThrowIfNull(bank);
        ArgumentNullException.ThrowIfNull(_clientId);
        _destinationAccountId = destinationAccountId;
        _money = money;
        _bank = bank;
        _clientId = clientId;
    }

    public Guid Id { get; } = Guid.NewGuid();
    public bool IsCanceled { get; private set; }

    public void Cancel()
    {
        if (IsCanceled)
            throw new TransactionException("this transaction has already been cancelled");
        _bank.Withdraw(_clientId, _destinationAccountId, _money);
        IsCanceled = true;
    }
}