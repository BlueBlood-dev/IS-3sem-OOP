namespace Banks.Models;

public class BankCountingInformation
{
    private const decimal MinDebitPercentageAmount = 0;
    private const decimal MinSuspiciousLimits = 0;
    private const decimal MinDepositPercentage = 0;
    private const decimal FirstAmountOfMoneyToGetPercentage = 50000;
    private const decimal SecondAmountOfMoneyToGetPercentage = 100000;
    private const decimal MinAllowedCreditCommission = 0;
    private const decimal MinAllowedCreditLimit = 0;

    public BankCountingInformation(
        decimal debitAccountRemainingMoneyPercentage,
        decimal suspiciousLimits,
        decimal smallestDepositPercentage,
        decimal middleDepositPercentage,
        decimal lastDepositPercentage,
        decimal creditCommission,
        decimal creditLimit)
    {
        if (debitAccountRemainingMoneyPercentage < MinDebitPercentageAmount)
            throw new ArgumentException($"debit percentage must be bigger than {MinDebitPercentageAmount}");
        if (suspiciousLimits < MinSuspiciousLimits)
            throw new ArgumentException($"limits should be greater than {MinSuspiciousLimits}");
        DebitAccountRemainingMoneyPercentage = debitAccountRemainingMoneyPercentage;
        SuspiciousLimits = suspiciousLimits;
        if (!(smallestDepositPercentage < middleDepositPercentage && middleDepositPercentage < lastDepositPercentage))
            throw new ArgumentException("deposit percentage should grow from smallest to largest");
        if (smallestDepositPercentage < MinDepositPercentage || middleDepositPercentage < MinDepositPercentage ||
            lastDepositPercentage < MinDepositPercentage)
            throw new ArgumentException($"deposit percentage should be greater than {MinDepositPercentage}");
        if (creditCommission < MinAllowedCreditCommission)
            throw new ArgumentException("CreditCommission should be greater than zero");
        if (creditLimit > MinAllowedCreditLimit)
            throw new ArgumentException("credit Limit should show how deep can user go below zero");
        SmallestDepositPercentage = smallestDepositPercentage;
        MiddleDepositPercentage = middleDepositPercentage;
        LastDepositPercentage = lastDepositPercentage;
        CreditCommission = creditCommission;
        CreditLimit = creditLimit;
    }

    public decimal DebitAccountRemainingMoneyPercentage { get; private set; }
    public decimal SuspiciousLimits { get; private set; }

    public decimal SmallestDepositPercentage { get; private set; }

    public decimal MiddleDepositPercentage { get; private set; }

    public decimal LastDepositPercentage { get; private set; }

    public decimal CreditCommission { get; private set; }

    public decimal CreditLimit { get; private set; }

    public void SetDebitAccountRemainingMoneyPercentage(decimal percentage)
    {
        if (percentage < MinDebitPercentageAmount)
            throw new ArgumentException($"debit percentage must be bigger than {MinDebitPercentageAmount}");
        DebitAccountRemainingMoneyPercentage = percentage;
    }

    public void SetSuspiciousLimits(decimal limits)
    {
        if (limits < MinSuspiciousLimits)
            throw new ArgumentException($"limits should be greater than {MinSuspiciousLimits}");
        SuspiciousLimits = limits;
    }

    public void SetSmallestDepositPercentage(decimal percentage)
    {
        if (percentage < MinDepositPercentage)
            throw new ArgumentException("percentage should be greater than zero");
        if (percentage > MiddleDepositPercentage)
        {
            throw new AggregateException(
                "The smallest percentage must be less than the middle percentage");
        }

        SmallestDepositPercentage = percentage;
    }

    public void SetMiddleDepositPercentage(decimal percentage)
    {
        if (percentage < MinDepositPercentage)
            throw new ArgumentException("percentage should be greater than zero");
        if (percentage < SmallestDepositPercentage || percentage > LastDepositPercentage)
        {
            throw new AggregateException(
                "The Middle percentage must be greater than the smallest percentage and less than the last percantage");
        }

        MiddleDepositPercentage = percentage;
    }

    public void SetLastDepositPercentage(decimal percentage)
    {
        if (percentage < MinDepositPercentage)
            throw new ArgumentException("percentage should be greater than zero");
        if (percentage < MiddleDepositPercentage)
        {
            throw new AggregateException(
                "The last percentage must be greater than the middle percentage");
        }

        LastDepositPercentage = percentage;
    }

    public void SetCreditCommission(decimal percentage)
    {
        if (percentage < MinAllowedCreditCommission)
            throw new ArgumentException("CreditCommission should be greater than zero");
        CreditCommission = percentage;
    }

    public void SetCreditLimit(decimal limit)
    {
        if (limit > MinAllowedCreditLimit)
            throw new ArgumentException("credit Limit should show how deep can user go below zero");
        CreditLimit = limit;
    }

    public decimal GetDepositPercentage(decimal money)
    {
        if (money < FirstAmountOfMoneyToGetPercentage)
            return SmallestDepositPercentage;
        return money < SecondAmountOfMoneyToGetPercentage ? MiddleDepositPercentage : LastDepositPercentage;
    }
}