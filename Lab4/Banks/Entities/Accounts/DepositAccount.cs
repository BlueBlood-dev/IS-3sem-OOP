using Banks.Exceptions;

namespace Banks.Entities.Accounts;

public class DepositAccount : IBankAccount
{
    private const decimal AllowedPercentage = 0;
    private const decimal AllowedSuspiciousLimit = 0;
    private const decimal AllowedAmountOfMoney = 0;
    private const int DaysToAccrue = 30;

    public DepositAccount(DateTime expirationTime, decimal depositPercentage, decimal suspiciousLimits, bool isSuspicious, decimal money, Guid clientId)
    {
        ArgumentNullException.ThrowIfNull(expirationTime);
        ExpirationTime = expirationTime;
        if (depositPercentage < AllowedPercentage)
            throw new ArgumentException("deposit percentage is wrong");
        DepositPercentage = depositPercentage;
        if (suspiciousLimits < AllowedSuspiciousLimit)
            throw new ArgumentException("entered suspicious limit is invalid");
        SuspiciousLimits = suspiciousLimits;
        IsSuspicious = isSuspicious;
        if (Money < AllowedAmountOfMoney)
            throw new ArgumentException("Money should not be less than zero");
        Money = money;
        ClientId = clientId;
        Id = Guid.NewGuid();
    }

    public decimal DepositPercentage { get; }
    public decimal Money { get; private set; }

    public int Days { get; private set; } = 0;
    public Guid Id { get; }
    public Guid ClientId { get; }
    public decimal AccruedInterest { get; private set; } = 0;
    public DateTime ExpirationTime { get; }
    public decimal SuspiciousLimits { get; }
    public bool IsSuspicious { get; }

    public void PutMoney(decimal money)
    {
        if (Money < AllowedAmountOfMoney)
            throw new TransactionException("sum to put on account should be greater than zero");
        Money += money;
    }

    public void WithdrawMoney(decimal money)
    {
        if (DateTime.Now < ExpirationTime)
            throw new TransactionException("can't withdraw money due to expire time hasn't come yet");
        if (Money - money < AllowedAmountOfMoney)
            throw new TransactionException("can't get below zero money amount on deposit account");
        if (IsSuspicious && money > SuspiciousLimits)
            throw new TransactionException("suspicious client can't withdraw so many money from his account");
        Money -= money;
    }

    public void AccrueInterest()
    {
        AccruedInterest += Money * DepositPercentage / 100;
    }

    public void IncreaseBalanceWithInterest()
    {
        if (Days != DaysToAccrue) return;
        Money += AccruedInterest;
        Days = 0;
    }

    public void SimulateDays(int days)
    {
        for (int i = 0; i < days; i++)
        {
            AccrueInterest();
            Days++;
            IncreaseBalanceWithInterest();
        }
    }
}