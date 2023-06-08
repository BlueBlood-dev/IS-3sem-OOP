namespace Banks.Console.Commands;

public class HelpCommand : ICommand
{
    public void Execute()
    {
        System.Console.WriteLine("These are available commands -------");
        System.Console.WriteLine("1. Create and Register Client");
        System.Console.WriteLine("2. Create Bank");
        System.Console.WriteLine("3. add debit account");
        System.Console.WriteLine("4. add credit account");
        System.Console.WriteLine("5. add deposit account");
        System.Console.WriteLine("6. replenish");
        System.Console.WriteLine("7. withdraw");
        System.Console.WriteLine("8. transfer");
        System.Console.WriteLine("9. help");
        System.Console.WriteLine("10. simulate days");
        System.Console.WriteLine("11. cancel transaction");
    }
}