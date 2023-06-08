using Banks.Exceptions;

namespace Banks.Entities.Accounts;

public class DebitAccount : IBankAccount
{
    private const decimal AllowedAmountOfMoneyToExecuteTransaction = 0;
    private const decimal AllowedAmountOfMoney = 0;
    private const int AllowedAmountOfSimulationDays = 0;
    private const int AllowedSuspiciousLimits = 0;
    private const int AllowedAmountOfRemainingMoneyPercentage = 0;
    private const int DaysToAccrue = 30;

    public DebitAccount(decimal remainingMoneyPercentage, Guid clientId, decimal suspiciousLimits, bool isSuspicious)
    {
        ArgumentNullException.ThrowIfNull(clientId);
        if (remainingMoneyPercentage < AllowedAmountOfRemainingMoneyPercentage)
            throw new ArgumentException("can't create account with such percents");
        if (suspiciousLimits < AllowedSuspiciousLimits)
            throw new ArgumentException("can't create account with such suspicious limits");
        RemainingMoneyPercentage = remainingMoneyPercentage;
        ClientId = clientId;
        SuspiciousLimits = suspiciousLimits;
        IsSuspicious = isSuspicious;
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }

    public Guid ClientId { get; }
    public bool IsSuspicious { get; }

    public uint Days { get; private set; } = 0;
    public decimal SuspiciousLimits { get; }

    public decimal RemainingMoneyPercentage { get; }

    public decimal Money { get; private set; } = 0;

    public decimal AccruedInterest { get; private set; } = 0;

    public void PutMoney(decimal money)
    {
        if (money <= AllowedAmountOfMoneyToExecuteTransaction)
            throw new TransactionException("you can't put such value on debit account");
        Money += money;
    }

    public void WithdrawMoney(decimal money)
    {
        if (money <= AllowedAmountOfMoneyToExecuteTransaction)
            throw new TransactionException("you can't withdraw such amount of money");
        if (Money - money < AllowedAmountOfMoney)
            throw new TransactionException("you can't get below zero on your debit account");
        if (IsSuspicious && money > SuspiciousLimits)
            throw new TransactionException("suspicious client can't withdraw so many money from his account");
        Money -= money;
    }

    public void AccrueInterest()
    {
        AccruedInterest += Money * RemainingMoneyPercentage / 100;
    }

    public void IncreaseBalanceWithInterest()
    {
        if (Days != DaysToAccrue) return;
        Money += AccruedInterest;
        Days = 0;
    }

    public void SimulateDays(int days)
    {
        if (days < AllowedAmountOfSimulationDays)
        {
            throw new ArgumentException(
                $"days to simulate should be greater than {AllowedAmountOfMoneyToExecuteTransaction}");
        }

        for (int i = 0; i < days; i++)
        {
            AccrueInterest();
            Days++;
            IncreaseBalanceWithInterest();
        }
    }
}