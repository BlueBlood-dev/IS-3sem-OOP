using Banks.Entities.Banks;

namespace Banks.Console.Commands;

public class CancelTransactionCommand : ICommand
{
    public void Execute()
    {
        System.Console.WriteLine("enter transaction id");
        var transactionId = new Guid(System.Console.ReadLine() ??
                                 throw new ArgumentException("transaction id can't be null"));
        CentralBank.GetInstance().CancelTransaction(transactionId);
        System.Console.WriteLine("transaction was cancelled");
    }
}