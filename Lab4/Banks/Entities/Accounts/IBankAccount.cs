namespace Banks.Entities.Accounts;

public interface IBankAccount
{
    Guid Id { get; }
    Guid ClientId { get; }
    decimal Money { get; }
    void PutMoney(decimal money);
    void WithdrawMoney(decimal money);

    void SimulateDays(int days);
}