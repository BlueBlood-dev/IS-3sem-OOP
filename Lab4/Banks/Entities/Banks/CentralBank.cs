using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Exceptions;
using Banks.Models;

namespace Banks.Entities.Banks;

public class CentralBank
{
    private static CentralBank? _instance;
    private readonly List<Bank> _registeredBanks;
    private readonly List<ITransaction> _transactions;

    private CentralBank()
    {
        _transactions = new List<ITransaction>();
        _registeredBanks = new List<Bank>();
    }

    public IReadOnlyCollection<Bank> Banks => _registeredBanks.AsReadOnly();
    public IReadOnlyCollection<ITransaction> Transactions => _transactions.AsReadOnly();

    public static CentralBank GetInstance()
    {
        return _instance ??= new CentralBank();
    }

    public Guid RegisterBank(BankCountingInformation information, string name)
    {
        var bank = new Bank(name, information);
        _registeredBanks.Add(bank);
        return bank.Id;
    }

    public Guid RegisterClientToBank(Client client, Guid bankId)
    {
        Bank? bank = GetBankById(bankId);
        if (bank is null)
            throw BankException.BankNotExistException("such bank doesn't exist");
        bank.AddClientToBank(client);
        return client.Id;
    }

    public Guid CreateDebitAccount(Guid bankId, Guid clientId)
    {
        Bank? bank = GetBankById(bankId);
        if (bank is null)
            throw BankException.BankNotExistException("such bank doesn't exist");
        return bank.AddDebitAccount(clientId);
    }

    public Guid CreateDepositAccount(Guid bankId, Guid clientId, decimal money, DateTime howLong)
    {
        Bank? bank = GetBankById(bankId);
        if (bank is null)
            throw BankException.BankNotExistException("such bank doesn't exist");
        return bank.AddDepositAccount(clientId, howLong, money);
    }

    public Guid CreateCreditAccount(Guid bankId, Guid clientId, decimal money)
    {
        Bank? bank = GetBankById(bankId);
        if (bank is null)
            throw BankException.BankNotExistException("such bank doesn't exist");
        return bank.AddCreditAccount(clientId, money);
    }

    public Guid MakeReplenishTransaction(Guid clientId, Guid accountId, Guid bankId, decimal money)
    {
        Bank? bank = GetBankById(bankId);
        if (bank is null)
            throw BankException.BankNotExistException("such bank doesn't exist");
        bank.Replenish(clientId, accountId, money);
        var replenishTransaction = new ReplenishTransaction(accountId, money, bank, clientId);
        _transactions.Add(replenishTransaction);
        return replenishTransaction.Id;
    }

    public Guid MakeWithdrawTransaction(Guid clientId, Guid accountId, Guid bankId, decimal money)
    {
        Bank? bank = GetBankById(bankId);
        if (bank is null)
            throw BankException.BankNotExistException("such bank doesn't exist");
        bank.Withdraw(clientId, accountId, money);
        var withdrawTransaction = new WithdrawTransaction(accountId, money, bank, clientId);
        _transactions.Add(withdrawTransaction);
        return withdrawTransaction.Id;
    }

    public Guid MakeTransferTransaction(
        Guid firstClientId,
        Guid fromAccountId,
        Guid fromBankId,
        Guid secondClientId,
        Guid toAccountId,
        Guid toBankId,
        decimal money)
    {
        Bank? firstBank = GetBankById(fromBankId);
        if (firstBank is null)
            throw BankException.BankNotExistException("such bank doesn't exist");
        Bank? secondBank = GetBankById(toBankId);
        if (secondBank is null)
            throw BankException.BankNotExistException("such bank doesn't exist");
        firstBank.Withdraw(firstClientId, fromAccountId, money);
        secondBank.Replenish(secondClientId, toAccountId, money);
        var transaction = new TransferTransaction(firstBank, firstClientId, fromAccountId, money, secondBank, secondClientId, toAccountId);
        _transactions.Add(transaction);
        return transaction.Id;
    }

    public void CancelTransaction(Guid id)
    {
        ArgumentNullException.ThrowIfNull(id);
        ITransaction? transaction = _transactions.FirstOrDefault(tr => tr.Id == id);
        ArgumentNullException.ThrowIfNull(transaction);
        transaction.Cancel();
    }

    public void PredictAccountFuture(int days, Guid accountId, Guid bankId)
    {
        Bank? bank = GetBankById(bankId);
        if (bank is null)
            throw BankException.BankNotExistException("such bank doesn't exist");
    }

    public IReadOnlyCollection<Client> GetClientsListByBankId(Guid id) =>
        _registeredBanks.FirstOrDefault(b => b.Id == id)?.Clients ?? throw new ArgumentException("there is no such bank");

    public Bank? GetBankById(Guid id) => _registeredBanks.FirstOrDefault(b => b.Id == id);
}