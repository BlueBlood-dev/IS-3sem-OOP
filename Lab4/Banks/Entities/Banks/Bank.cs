using System.Transactions;
using Banks.Entities.Accounts;
using Banks.Entities.Clients;
using Banks.Exceptions;
using Banks.Models;
using Banks.Observer;
using TransactionException = Banks.Exceptions.TransactionException;

namespace Banks.Entities.Banks;

public class Bank : IObservable
{
    private readonly List<Client> _clients = new List<Client>();
    private readonly List<IObserver> _observers = new List<IObserver>();
    private readonly List<IBankAccount> _accounts = new List<IBankAccount>();

    public Bank(string bankName, BankCountingInformation countingInformation)
    {
        ArgumentNullException.ThrowIfNull(countingInformation);
        if (string.IsNullOrWhiteSpace(bankName))
            throw new ArgumentException("invalid bank name, try another one");
        BankName = bankName;
        CountingInformation = countingInformation;
        Id = Guid.NewGuid();
    }

    public BankCountingInformation CountingInformation { get; }

    public Guid Id { get; }
    public string BankName { get; }

    public IReadOnlyCollection<Client> Clients => _clients.AsReadOnly();
    public IReadOnlyCollection<IBankAccount> Accounts => _accounts.AsReadOnly();
    public IReadOnlyCollection<IObserver> SubscribedClients => _observers.AsReadOnly();

    public bool CheckIfBankAlreadyHasSuchClient(Guid id) =>
        _clients.Any(cl =>
            id == cl.Id);

    public bool CheckIfClientIsSuspicious(Guid id)
    {
        Client? client = _clients.FirstOrDefault(cl => cl.Id == id);
        ArgumentNullException.ThrowIfNull(client);
        return client.Address is null && client.Passport is null;
    }

    public Client AddClientToBank(Client client)
    {
        ArgumentNullException.ThrowIfNull(client);
        if (CheckIfBankAlreadyHasSuchClient(client.Id))
            throw new ClientException("such client already exists");
        _clients.Add(client);
        return client;
    }

    public Guid AddDebitAccount(Guid clientId)
    {
        ArgumentNullException.ThrowIfNull(clientId);
        if (!CheckIfBankAlreadyHasSuchClient(clientId))
            throw new ClientException("Client should be registered in bank before creating an account");
        var debitAccount = new DebitAccount(
            CountingInformation.DebitAccountRemainingMoneyPercentage,
            clientId,
            CountingInformation.SuspiciousLimits,
            CheckIfClientIsSuspicious(clientId));
        _accounts.Add(debitAccount);
        return debitAccount.Id;
    }

    public Guid AddDepositAccount(Guid clientId, DateTime expirationTime, decimal money)
    {
        ArgumentNullException.ThrowIfNull(clientId);
        if (!CheckIfBankAlreadyHasSuchClient(clientId))
            throw new ClientException("Client should be registered in bank before creating an account");
        var depositAccount = new DepositAccount(
            expirationTime,
            CountingInformation.GetDepositPercentage(money),
            CountingInformation.SuspiciousLimits,
            CheckIfClientIsSuspicious(clientId),
            money,
            clientId);
        _accounts.Add(depositAccount);
        return depositAccount.Id;
    }

    public Guid AddCreditAccount(Guid clientId, decimal money)
    {
        ArgumentNullException.ThrowIfNull(clientId);
        if (!CheckIfBankAlreadyHasSuchClient(clientId))
            throw new ClientException("Client should be registered in bank before creating an account");
        var creditAccount = new CreditAccount(
            CountingInformation.CreditCommission,
            money,
            CheckIfClientIsSuspicious(clientId),
            CountingInformation.CreditLimit,
            clientId,
            CountingInformation.SuspiciousLimits);
        _accounts.Add(creditAccount);
        return creditAccount.Id;
    }

    public void Replenish(Guid id, Guid accountId, decimal money)
    {
        IBankAccount? account = _accounts.FirstOrDefault(a => a.Id == accountId);
        ArgumentNullException.ThrowIfNull(account);
        if (account.ClientId != id)
            throw new TransactionException("client doesn't own this account");
        account.PutMoney(money);
    }

    public void Withdraw(Guid id, Guid accountId, decimal money)
    {
        IBankAccount? account = _accounts.FirstOrDefault(a => a.Id == accountId);
        ArgumentNullException.ThrowIfNull(account);
        if (account.ClientId != id)
            throw new TransactionException("client doesn't own this account");
        account.WithdrawMoney(money);
    }

    public void AddObserver(IObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        if (!CheckIfBankAlreadyHasSuchClient(observer.ReturnObserverId()))
            throw new ClientException("observer must be bank's client");
        _observers.Add(observer);
    }

    public void SimulateTIme(Guid id, int days)
    {
        IBankAccount? account = _accounts.FirstOrDefault(a => a.Id == id);
        ArgumentNullException.ThrowIfNull(account);
        account.SimulateDays(days);
    }

    public void RemoveObserver(IObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);
        if (!CheckIfBankAlreadyHasSuchClient(observer.ReturnObserverId()))
            throw new ClientException("there is no such client to remove from notifications system");
        _observers.Remove(observer);
    }

    public void Notify(string message)
    {
        _observers.ForEach(o => o.Update(message));
    }

    public void SetNewDebitPercentage(decimal percentage)
    {
        CountingInformation.SetDebitAccountRemainingMoneyPercentage(percentage);
        Notify("new DebitAccountRemainingMoneyPercentage was set, check out in your profile!");
    }

    public void SetNewSuspiciousLimits(decimal limits)
    {
        CountingInformation.SetSuspiciousLimits(limits);
        Notify("new SuspiciousLimits were set, check out in your profile!");
    }

    public void SetSmallestDepositPercentage(decimal percentage)
    {
        CountingInformation.SetSmallestDepositPercentage(percentage);
        Notify("new smallest deposit percentage was set, check out in your profile");
    }

    public void SetMiddleDepositPercentage(decimal percentage)
    {
        CountingInformation.SetMiddleDepositPercentage(percentage);
        Notify("new smallest deposit percentage was set, check out in your profile");
    }

    public void SetLastDepositPercentage(decimal percentage)
    {
        CountingInformation.SetLastDepositPercentage(percentage);
        Notify("new smallest deposit percentage was set, check out in your profile");
    }

    public void SetCreditLimit(decimal limit)
    {
        CountingInformation.SetCreditLimit(limit);
        Notify("new credit limit was set, check out!");
    }

    public void SetCreditCommission(decimal percentage)
    {
        CountingInformation.SetCreditCommission(percentage);
        Notify("new credit commission was set, check out!");
    }
}