using Banks.Entities.Banks;
using Banks.Exceptions;

namespace Banks.Entities.Transactions;

public class TransferTransaction : ITransaction
{
    private readonly Bank _fromBank;
    private readonly Guid _firstClientId;
    private readonly Guid _fromAccountId;
    private readonly Bank _toBank;
    private readonly Guid _secondClientId;
    private readonly Guid _toAccountId;
    private readonly decimal _money;

    public TransferTransaction(Bank fromBank, Guid firstClientId, Guid fromAccountId, decimal money, Bank toBank, Guid secondClientId, Guid toAccountId)
    {
        ArgumentNullException.ThrowIfNull(fromBank);
        ArgumentNullException.ThrowIfNull(firstClientId);
        ArgumentNullException.ThrowIfNull(fromAccountId);
        ArgumentNullException.ThrowIfNull(toBank);
        ArgumentNullException.ThrowIfNull(secondClientId);
        ArgumentNullException.ThrowIfNull(toAccountId);
        _fromBank = fromBank;
        _firstClientId = firstClientId;
        _fromAccountId = fromAccountId;
        _money = money;
        _toBank = toBank;
        _secondClientId = secondClientId;
        _toAccountId = toAccountId;
    }

    public Guid Id { get; } = Guid.NewGuid();
    public bool IsCanceled { get; } = false;

    public void Cancel()
    {
        if (IsCanceled)
            throw new TransactionException("this transaction has been already cancelled");
        _fromBank.Replenish(_firstClientId, _fromAccountId, _money);
        _toBank.Withdraw(_secondClientId, _toAccountId, _money);
    }
}