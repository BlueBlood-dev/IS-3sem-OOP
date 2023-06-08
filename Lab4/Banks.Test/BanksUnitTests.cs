using System.Diagnostics;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Exceptions;
using Banks.Models;
using Banks.Models.Passport;
using Xunit;

namespace Banks.Test;

public class BanksUnitTests
{
    private readonly CentralBank _centralBank = CentralBank.GetInstance();
    [Fact]
    public void RegisterClient_ClientIsRegistered()
    {
        var countingInfo = new BankCountingInformation(1, 10000000, 1, 2, 3, 2, -1000);
        Guid bankId = _centralBank.RegisterBank(countingInfo, "MMM banking");
        _centralBank.RegisterClientToBank(
            Client.BuilderContext()
                .SetName("joe")
                .SetSurname("biden")
                .SetAddress(new Address("komenda"))
            .SetPassport(new RussianPassport("8966", "896555", "registered", "issuePlace")).Build(),
            bankId);
        Assert.Equal(1, _centralBank.GetClientsListByBankId(bankId).Count);
    }

    [Fact]
    public void RegisterBank_BankIsRegistered()
    {
        var countingInfo = new BankCountingInformation(1, 10000000, 1, 2, 3, 2, -1000);
        _centralBank.RegisterBank(countingInfo, "MMM banking");
        Assert.NotEqual(0, _centralBank.Banks.Count);
    }

    [Fact]
    public void CreateTransaction_TransactionCreated()
    {
        var countingInfo = new BankCountingInformation(1, 10000000, 1, 2, 3, 2, -1000);
        Guid bankId = _centralBank.RegisterBank(countingInfo, "MMM banking");
        Guid clientId = _centralBank.RegisterClientToBank(
            Client.BuilderContext()
                .SetName("joe")
                .SetSurname("biden")
                .SetAddress(new Address("komenda"))
                .SetPassport(new RussianPassport("8966", "896555", "registered", "issuePlace")).Build(),
            bankId);
        Guid accountId = _centralBank.CreateDebitAccount(bankId, clientId);
        _centralBank.MakeReplenishTransaction(clientId, accountId, bankId, 10000);
        Assert.Equal(2, _centralBank.Transactions.Count);
    }

    [Fact]
    public void CancelTransaction_TransactionCanceledThrowsTransactionExceptionWhenCancellingAgain()
    {
        var countingInfo = new BankCountingInformation(1, 10000000, 1, 2, 3, 2, -1000);
        Guid bankId = _centralBank.RegisterBank(countingInfo, "MMM banking");
        Guid clientId = _centralBank.RegisterClientToBank(
            Client.BuilderContext()
                .SetName("joe")
                .SetSurname("biden")
                .SetAddress(new Address("komenda"))
                .SetPassport(new RussianPassport("8966", "896555", "registered", "issuePlace")).Build(),
            bankId);
        Guid accountId = _centralBank.CreateDebitAccount(bankId, clientId);
        Guid transactionId = _centralBank.MakeReplenishTransaction(clientId, accountId, bankId, 10000);
        _centralBank.CancelTransaction(transactionId);
        Assert.Throws<TransactionException>(() => _centralBank.CancelTransaction(transactionId));
    }

    [Fact]
    public void ChangeCountingInformation_CountingInformationChanged()
    {
        var countingInfo = new BankCountingInformation(1, 10000000, 1, 2, 3, 2, -1000);
        decimal oldCreditCommission = countingInfo.CreditCommission;
        Guid bankId = _centralBank.RegisterBank(countingInfo, "MMM banking");
        Bank? bank = _centralBank.GetBankById(bankId);
        Debug.Assert(bank != null, nameof(bank) + " != null");
        bank.SetCreditCommission(10);
        Assert.NotEqual(countingInfo.CreditCommission, oldCreditCommission);
    }

    [Fact]
    public void MakeReplenishTransaction_MoneyIncreased()
    {
        var countingInfo = new BankCountingInformation(1, 10000000, 1, 2, 3, 2, -1000);
        Guid bankId = _centralBank.RegisterBank(countingInfo, "MMM banking");
        Bank? bank = _centralBank.GetBankById(bankId);
        Guid clientId = _centralBank.RegisterClientToBank(
            Client.BuilderContext()
                .SetName("joe")
                .SetSurname("biden")
                .SetAddress(new Address("komenda"))
                .SetPassport(new RussianPassport("8966", "896555", "registered", "issuePlace")).Build(),
            bankId);
        Guid accountId = _centralBank.CreateDebitAccount(bankId, clientId);
        _centralBank.MakeReplenishTransaction(clientId, accountId, bankId, 10000);
        Debug.Assert(bank != null, nameof(bank) + " != null");
        Assert.True(bank.Accounts.All(a => a.Money == 10000));
    }

    [Fact]
    public void MakeWithdrawTransaction_MoneyDecreased()
    {
        var countingInfo = new BankCountingInformation(1, 10000000, 1, 2, 3, 2, -1000);
        Guid bankId = _centralBank.RegisterBank(countingInfo, "MMM banking");
        Bank? bank = _centralBank.GetBankById(bankId);
        Guid clientId = _centralBank.RegisterClientToBank(
            Client.BuilderContext()
                .SetName("joe")
                .SetSurname("biden")
                .SetAddress(new Address("komenda"))
                .SetPassport(new RussianPassport("8966", "896555", "registered", "issuePlace")).Build(),
            bankId);
        Guid accountId = _centralBank.CreateDebitAccount(bankId, clientId);
        _centralBank.MakeReplenishTransaction(clientId, accountId, bankId, 10000);
        _centralBank.MakeWithdrawTransaction(clientId, accountId, bankId, 1000);
        Assert.True(bank != null && bank.Accounts.All(a => a.Money == 9000));
    }

    [Fact]
    public void SubscribeClient_ClientSubscribed()
    {
        var countingInfo = new BankCountingInformation(1, 10000000, 1, 2, 3, 2, -1000);
        Guid bankId = _centralBank.RegisterBank(countingInfo, "MMM banking");
        Bank? bank = _centralBank.GetBankById(bankId);
        Client client = Client.BuilderContext()
            .SetName("joe")
            .SetSurname("biden")
            .SetAddress(new Address("komenda"))
            .SetPassport(new RussianPassport("8966", "896555", "registered", "issuePlace")).Build();
        Guid clientId = _centralBank.RegisterClientToBank(client, bankId);
        Debug.Assert(bank != null, nameof(bank) + " != null");
        bank.AddObserver(client);
        Assert.Equal(1, bank.SubscribedClients.Count);
    }
}