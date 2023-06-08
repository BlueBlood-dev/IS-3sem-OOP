using Banks.Exceptions;

namespace Banks.Entities.Accounts;

public class CreditAccount : IBankAccount
{
    private const decimal MinAllowedAmountOfMoney = 0;
    private const decimal MinAllowedCreditLimit = 0;
    private const decimal MinAllowedCreditCommission = 0;
    private const decimal MinAllowedSuspiciousLimits = 0;

    public CreditAccount(decimal creditCommission, decimal money, bool isSuspicious, decimal creditLimit, Guid clientId, decimal suspiciousLimits)
    {
        ArgumentNullException.ThrowIfNull(clientId);
        if (creditCommission < MinAllowedCreditCommission)
            throw new ArgumentException("credit commission should be greater than zero");
        if (Money < MinAllowedAmountOfMoney)
            throw new ArgumentException("money amount should be greater than zero");
        if (creditLimit > MinAllowedCreditLimit)
            throw new ArgumentException("credit limit should show hod deep below zero can client go");
        if (suspiciousLimits < MinAllowedSuspiciousLimits)
            throw new AggregateException("suspicious limits must be greater than zero");
        CreditCommission = creditCommission;
        Money = money;
        IsSuspicious = isSuspicious;
        CreditLimit = creditLimit;
        ClientId = clientId;
        SuspiciousLimits = suspiciousLimits;
        Id = Guid.NewGuid();
    }

    public decimal CreditCommission { get; }
    public decimal CreditLimit { get; }
    public decimal SuspiciousLimits { get; }
    public bool IsSuspicious { get; }
    public decimal Money { get; private set; }
    public Guid Id { get; }
    public Guid ClientId { get; }

    public bool DebtState { get; private set; } = false;

    public void PutMoney(decimal money)
    {
        if (DebtState)
            money -= Money * CreditCommission / 100;
        if (money < MinAllowedAmountOfMoney)
            throw new ArgumentException("wrong amount of money to put in");
        Money += money;
        if (Money > 0)
            DebtState = false;
    }

    public void WithdrawMoney(decimal money)
    {
        if (DebtState)
            money -= CreditCommission * Money / 100;
        if (Money - money < CreditLimit)
            throw new TransactionException("can't go below credit limit");
        if (IsSuspicious && money > SuspiciousLimits)
            throw new TransactionException("suspicious client can't withdraw so many money from his account");
        Money -= money;
        if (Money < MinAllowedAmountOfMoney)
            DebtState = true;
    }

    public void SimulateDays(int days)
    {
    }
}