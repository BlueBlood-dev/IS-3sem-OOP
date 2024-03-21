using Banks.Entities.Banks;
using Banks.Models;

namespace Banks.Console.Commands;

public class CreateBankCommand : ICommand
{
    public void Execute()
    {
        System.Console.WriteLine("enter suspicious limits");
        decimal limits = Convert.ToDecimal(System.Console.ReadLine());
        System.Console.WriteLine("enter debit account percentage");
        decimal debitPercentage = Convert.ToDecimal(System.Console.ReadLine());
        System.Console.WriteLine("enter smallest deposit percentage");
        decimal smallest = Convert.ToDecimal(System.Console.ReadLine());
        System.Console.WriteLine("enter middle deposit percentage");
        decimal middle = Convert.ToDecimal(System.Console.ReadLine());
        System.Console.WriteLine("enter last deposit percentage");
        decimal last = Convert.ToDecimal(System.Console.ReadLine());
        System.Console.WriteLine("enter credit commission");
        decimal creditCommission = Convert.ToDecimal(System.Console.ReadLine());
        System.Console.WriteLine("enter credit limits");
        decimal creditLimits = Convert.ToDecimal(System.Console.ReadLine());
        var bankInformation = new BankCountingInformation(debitPercentage, limits, smallest, middle, last, creditCommission, creditLimits);
        System.Console.WriteLine("enter bank name");
        string bankName = System.Console.ReadLine() ?? throw new ArgumentException("can't create bank with null name");
        Guid guid = CentralBank.GetInstance().RegisterBank(bankInformation, bankName);
        System.Console.WriteLine($"bank was successfully registered, bank ID is : {guid}");
    }
}