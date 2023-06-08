﻿using Banks.Entities.Banks;

namespace Banks.Console.Commands;

public class CreateDepositAccountCommand : ICommand
{
    public void Execute()
    {
        System.Console.WriteLine("enter client id");
        var clientId = new Guid(System.Console.ReadLine() ??
                            throw new ArgumentException("user id can't be null"));
        System.Console.WriteLine("enter bank id");
        var bankId = new Guid(System.Console.ReadLine() ??
                          throw new ArgumentException("user id can't be null"));
        System.Console.WriteLine("enter for how many months you want to create deposit account");
        int months = Convert.ToInt32(System.Console.ReadLine());
        System.Console.WriteLine("enter how much money you went tu put on your account");
        decimal money = Convert.ToDecimal(System.Console.ReadLine());
        Guid accountId = CentralBank.GetInstance().CreateDepositAccount(bankId, clientId, money, DateTime.Now.AddMonths(months));
        System.Console.WriteLine($"debit account was successfully created, it's id is {accountId}");
    }
}