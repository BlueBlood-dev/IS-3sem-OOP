using Banks.Entities.Banks;

namespace Banks.Console.Commands;

public class CreateWithdrawTransactionCommand : ICommand
{
    public void Execute()
    {
        System.Console.WriteLine("enter client id");
        var clientId = new Guid(System.Console.ReadLine() ??
                            throw new ArgumentException("user id can't be null"));
        System.Console.WriteLine("enter bank id");
        var bankId = new Guid(System.Console.ReadLine() ??
                          throw new ArgumentException("bank id can't be null"));
        System.Console.WriteLine("enter account id");
        var accountId = new Guid(System.Console.ReadLine() ??
                             throw new ArgumentException("account id can't be null"));
        System.Console.WriteLine("enter how much money you want to get from account");
        decimal money = Convert.ToDecimal(System.Console.ReadLine());
        Guid transactionId = CentralBank.GetInstance().MakeWithdrawTransaction(clientId, accountId, bankId, money);
        System.Console.WriteLine($"withdraw transaction was created. it's id is: {transactionId}");
    }
}