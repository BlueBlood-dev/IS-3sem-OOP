using Banks.Entities.Banks;

namespace Banks.Console.Commands;

public class CreateDebitAccountCommand : ICommand
{
    public void Execute()
    {
        System.Console.WriteLine("enter client id");
        var clientId = new Guid(System.Console.ReadLine() ??
                            throw new ArgumentException("user id can't be null"));
        System.Console.WriteLine("enter bank id");
        var bankId = new Guid(System.Console.ReadLine() ??
                          throw new ArgumentException("nak id can't be null"));
        Guid accountId = CentralBank.GetInstance().CreateDebitAccount(bankId, clientId);
        System.Console.WriteLine($"debit account was successfully created, it's id is {accountId}");
    }
}