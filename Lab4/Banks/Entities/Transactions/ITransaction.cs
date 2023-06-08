namespace Banks.Entities.Transactions;

public interface ITransaction
{
    Guid Id { get; }
    bool IsCanceled { get; }
    void Cancel();
}