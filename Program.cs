// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using System;
using System.Collections.Generic;
using System.Linq; 

 /// NetProgramming
 /// Joshua Vachachira
 
// Base class
abstract class Investment
{
    public string CustomerName { get; set; }
    public string ID { get; set; }
    private DateTime openingDate;
    public decimal Balance { get; set; }

    public string OpeningDate
    {
        get { return openingDate.ToString("MM/dd/yyyy"); }
        set
        {
            if (DateTime.TryParse(value, out DateTime parsedDate))
                openingDate = parsedDate;
            else
                throw new Exception("Invalid date format. Please enter in MM/DD/YYYY.");
        }
    }

    public Investment(string name, string id, string openingDate, decimal balance)
    {
        CustomerName = name;
        ID = id;
        OpeningDate = openingDate;
        Balance = balance;
    }

    public abstract void ApplyAdjustment();
    public abstract string GetInvestmentType();
    public override string ToString()
    {
        return $"Type={GetInvestmentType()}, ID={ID}, Name={CustomerName}, Opening Date={OpeningDate}, Balance=${Balance:0.00}";
    }
}

// CheckingAccount class
class CheckingAccount : Investment
{
    public decimal OverdraftFee { get; set; }

    public CheckingAccount(string name, string id, string openingDate, decimal balance, decimal overdraftFee)
        : base(name, id, openingDate, balance)
    {
        OverdraftFee = overdraftFee;
    }

    public void Withdraw(decimal amount)
    {
        Balance -= amount;
    }

    public void Deposit(decimal amount)
    {
        Balance += amount;
    }

    public override void ApplyAdjustment()
    {
        if (Balance < 0)
        {
            Console.WriteLine("Overdraft fee applied.");
            Balance -= OverdraftFee;
        }
    }

    public override string GetInvestmentType() => "Checking";
    public override string ToString() => base.ToString() + $", Overdraft Fee=${OverdraftFee:0.00}";
}

// CD class
class CD : Investment
{
    public decimal InterestRate { get; set; }

    public CD(string name, string id, string openingDate, decimal balance, decimal interestRate)
        : base(name, id, openingDate, balance)
    {
        InterestRate = interestRate;
    }

    public override void ApplyAdjustment()
    {
        Balance += Balance * (InterestRate / 100);
    }

    public override string GetInvestmentType() => "CD";
    public override string ToString() => base.ToString() + $", Interest Rate={InterestRate:0.00}%";
}

// Main program
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("************************************************************");
        Console.WriteLine("               INVESTMENT TRACKER VERSION 1.0");
        Console.WriteLine("************************************************************\n");
        Console.WriteLine("\nThis tool helps you manage your investments, both CDs and checking accounts.");
        Console.WriteLine("CDs accrue interest and checking accounts can have overdraft fees.\n");


        Console.Write("Enter your name: ");
        string name = Console.ReadLine();

        // Create Checking Account
        CheckingAccount checking;
        while (true)
        {
            try
            {
                Console.Write("Enter Checking ID, Opening Date (MM/DD/YYYY), Balance, Overdraft Fee: ");
                string[] checkData = Console.ReadLine().Split(' ');
                checking = new CheckingAccount(name, checkData[0], checkData[1], decimal.Parse(checkData[2]), decimal.Parse(checkData[3]));
                break;
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
        }

        // Create CD
        CD cd;
        while (true)
        {
            try
            {
                Console.Write("Enter CD ID, Opening Date (MM/DD/YYYY), Balance, Interest Rate: ");
                string[] cdData = Console.ReadLine().Split(' ');
                cd = new CD(name, cdData[0], cdData[1], decimal.Parse(cdData[2]), decimal.Parse(cdData[3]));
                break;
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
        }

        List<Investment> investments = new List<Investment> { checking, cd };

        while (true)
        {
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1. Withdraw from checking");
            Console.WriteLine("2. Deposit into checking");
            Console.WriteLine("3. Update balances");
            Console.WriteLine("4. List investments");
            Console.WriteLine("5. Quit");
            Console.Write("Enter the number of your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("How much do you want to withdraw? ");
                    decimal withdrawAmount = decimal.Parse(Console.ReadLine());
                    checking.Withdraw(withdrawAmount);
                    break;
                case "2":
                    Console.Write("How much do you want to deposit? ");
                    decimal depositAmount = decimal.Parse(Console.ReadLine());
                    checking.Deposit(depositAmount);
                    break;
                case "3":
                    foreach (Investment inv in investments)
                        inv.ApplyAdjustment();
                    Console.WriteLine("The CD accrued interest, and the checking account applied late fees if applicable.");
                    break;
                case "4":
                    Console.WriteLine("Here are your investments at our bank:");
                    foreach (Investment inv in investments)
                        Console.WriteLine(inv);
                    break;
                case "5":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                 Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }
}



