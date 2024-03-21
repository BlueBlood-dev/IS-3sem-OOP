using Banks.Console.Commands;
using Banks.Entities.Banks;

namespace Banks.Console;

internal static class Program
{
    public static void Main(string[] args)
    {
        CentralBank.GetInstance();
        new HelpCommand().Execute();
        while (true)
        {
            string? command = System.Console.ReadLine();
            try
            {
                ICommand task;
                switch (command)
                {
                    case "1":
                        task = new RegisterClientCommand();
                        task.Execute();
                        break;
                    case "2":
                        task = new CreateBankCommand();
                        task.Execute();
                        break;
                    case "3":
                        task = new CreateDebitAccountCommand();
                        task.Execute();
                        break;
                    case "4":
                        task = new CreateCreditAccountCommand();
                        task.Execute();
                        break;
                    case "5":
                        task = new CreateDepositAccountCommand();
                        task.Execute();
                        break;
                    case "6":
                        task = new CreateReplenishTransactionCommand();
                        task.Execute();
                        break;
                    case "7":
                        task = new CreateWithdrawTransactionCommand();
                        task.Execute();
                        break;
                    case "8":
                        task = new CreateTransferTransactionCommand();
                        task.Execute();
                        break;
                    case "9":
                        task = new HelpCommand();
                        task.Execute();
                        break;
                    case "10":
                        task = new SimulationCommand();
                        task.Execute();
                        break;
                    case "11":
                        task = new CancelTransactionCommand();
                        task.Execute();
                        break;
                    default:
                        System.Console.WriteLine("there is no such command, type 9 to get help");
                        continue;
                }
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception);
            }
        }
    }
}