using Banks.Entities.Banks;

namespace Banks.Console.Commands;

public class CreateTransferTransactionCommand : ICommand
{
    public void Execute()
    {
        System.Console.WriteLine("enter first client id");
        var clientId = new Guid(System.Console.ReadLine() ??
                                throw new ArgumentException("bank id can't be null"));
        System.Console.WriteLine("enter first bank id");
        var bankId = new Guid(System.Console.ReadLine() ??
                              throw new ArgumentException("bank id can't be null"));
        System.Console.WriteLine("enter first account id");
        var accountId = new Guid(System.Console.ReadLine() ??
                                 throw new ArgumentException("account id can't be null"));
        System.Console.WriteLine("enter second client id");
        var clientIdExtra = new Guid(System.Console.ReadLine() ??
                                     throw new ArgumentException("user id can't be null"));
        System.Console.WriteLine("enter second bank id");
        var bankIdExtra = new Guid(System.Console.ReadLine() ??
                                   throw new ArgumentException("bank id can't be null"));
        System.Console.WriteLine("enter second account id");
        var accountIdExtra = new Guid(System.Console.ReadLine() ??
                                      throw new ArgumentException("account id can't be null"));
        System.Console.WriteLine("enter how much money you want to get from account");
        decimal money = Convert.ToDecimal(System.Console.ReadLine());
        Guid transactionId = CentralBank.GetInstance().MakeTransferTransaction(
            clientId,
            accountId,
            bankId,
            clientIdExtra,
            accountIdExtra,
            bankIdExtra,
            money);
        System.Console.WriteLine($"transfer transaction was created, it's id is: {transactionId}");
    }
}