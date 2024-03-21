using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Models;
using Banks.Models.Passport;

namespace Banks.Console.Commands;

public class RegisterClientCommand : ICommand
{
    public void Execute()
    {
        System.Console.WriteLine("enter client name");
        string name = System.Console.ReadLine() ?? throw new ArgumentException("name of client can't be null");
        System.Console.WriteLine("enter client surname");
        string surname = System.Console.ReadLine() ?? throw new ArgumentException("surname of client can't be null");
        FluentClientBuilder builder = Client.BuilderContext();
        System.Console.WriteLine($"Entered name is {name}, do you want to change it? y/n");
        string? changeName = System.Console.ReadLine();
        if (changeName == "y")
        {
            System.Console.WriteLine("enter new name");
            builder.SetName(System.Console.ReadLine() ?? throw new ArgumentException("name can't be null"));
        }

        System.Console.WriteLine($"Entered surname is {surname}, do you want to change it? y/n");
        string? changeSurname = System.Console.ReadLine();
        if (changeSurname == "y")
        {
            System.Console.WriteLine("enter new surname");
            builder.SetSurname(System.Console.ReadLine() ?? throw new ArgumentException("surname can't be null"));
        }

        System.Console.WriteLine("do you want to add address to your client? y/n");
        string? addressAnswer = System.Console.ReadLine();
        if (addressAnswer == "y")
        {
            System.Console.WriteLine("enter address");
            string address = System.Console.ReadLine() ?? throw new ArgumentException("address can't be null");
            builder.SetAddress(new Address(address));
        }

        System.Console.WriteLine("do you want to add passport to your client? y/n");
        string? passportAnswer = System.Console.ReadLine();
        if (passportAnswer == "y")
        {
            System.Console.WriteLine("enter passport number");
            string number = System.Console.ReadLine() ?? throw new ArgumentException("passport number can't be null");
            System.Console.WriteLine("enter serial number");
            string serial = System.Console.ReadLine() ??
                            throw new ArgumentException("passport serial can't be null");
            System.Console.WriteLine("enter serial number");
            string livingPlace = System.Console.ReadLine() ??
                                 throw new ArgumentException("passport living place can't be null");
            System.Console.WriteLine("enter serial number");
            string issuePlace = System.Console.ReadLine() ??
                                throw new ArgumentException("passport issue place can't be null");
            builder.SetPassport(new RussianPassport(serial, number, livingPlace, issuePlace));
        }

        Client client = builder.SetName(name).SetSurname(surname).Build();
        System.Console.WriteLine("enter bank id");
        var id = new Guid(System.Console.ReadLine() ?? throw new ArgumentException("bank id can't be null"));
        CentralBank.GetInstance().RegisterClientToBank(client, id);
        System.Console.WriteLine($"client was successfully registered, his client ID is : {client.Id}");
    }
}