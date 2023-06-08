using Banks.Entities.Banks;

namespace Banks.Console.Commands;

public class SimulationCommand : ICommand
{
    public void Execute()
    {
        System.Console.WriteLine("enter bank id");
        var bankId = new Guid(System.Console.ReadLine() ??
                          throw new ArgumentException("bank id can't be null"));
        System.Console.WriteLine("enter account id");
        var accountId = new Guid(System.Console.ReadLine() ??
                             throw new ArgumentException("account id can't be null"));
        System.Console.WriteLine("enter simulationTime");
        int days = Convert.ToInt32(System.Console.ReadLine());
        CentralBank.GetInstance().PredictAccountFuture(days, accountId, bankId);
        System.Console.WriteLine("-----Simulation was passed-----");
    }
}