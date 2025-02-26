// See https://aka.ms/new-console-template for more information

// Investment-TrackerV1.0
//Netprogramming

using System;
using System.Collections.Generic;

// The base class for investment

abstract class Investment
{
    public string CustomerName { get; set; }
    public string ID { get; set; }
    private DateTime openingDate;
    private decimal balance;

    // Encapsulated Balance property
    public decimal Balance
    {
        get { return balance; }
        protected set { balance = value; }
    }

    // Property to handle date formatting and validation
    public string OpeningDate
    {
        get { return openingDate.ToString("MM/dd/yyyy"); }
        set
        {
            if (!DateTime.TryParse(value, out openingDate))
            {
                throw new Exception("Invalid date format. Please enter date as M/d/yyyy.");
            }
        }
    }
    
    // Constructor for Investment base class
    public Investment(string customerName, string id, string openingDate, decimal balance)
    {
        CustomerName = customerName;
        ID = id;
        OpeningDate = openingDate;
        this.balance = balance;
    }
    
    public abstract void ApplyAdjustment();
    public abstract string GetInvestmentType();
    
    // Override ToString() to display account details
    public override string ToString()
    {
        return $"Type={GetInvestmentType()}, ID={ID}, Name of Holder={CustomerName}, Opening Date={OpeningDate}, Balance=${Balance:F2}";
    }
}

// Checking Account subclass
class CheckingAccount : Investment
{
    public decimal OverdraftFee { get; set; }

    public CheckingAccount(string customerName, string id, string openingDate, decimal balance, decimal overdraftFee)
        : base(customerName, id, openingDate, balance)
    {
        OverdraftFee = overdraftFee;
    }

    // Deposit method increases balance
    public void Deposit(decimal amount)
    {
        Balance += amount;
    }

    // Withdraw method with overdraft protection
    public void Withdraw(decimal amount)
    {
        if (Balance - amount < 0)
        {
            Console.WriteLine("Warning: This transaction will result in an overdraft!");
        }
        Balance -= amount;
    }

    // Apply overdraft fee if balance is negative
    public override void ApplyAdjustment()
    {
        if (Balance < 0)
        {
            Balance -= OverdraftFee;
        }
    }

    public override string GetInvestmentType() => "Checking";

    public override string ToString()
    {
        return base.ToString() + $", Overdraft Fee=${OverdraftFee:F2}";
    }
}

// CD subclass
class CD : Investment
{
    public decimal InterestRate { get; set; }

    public CD(string customerName, string id, string openingDate, decimal balance, decimal interestRate)
        : base(customerName, id, openingDate, balance)
    {
        InterestRate = interestRate;
    }

    // Apply interest to the CD balance
    public override void ApplyAdjustment()
    {
        Balance += Balance * (InterestRate / 100);
    }

    public override string GetInvestmentType() => "CD";

    public override string ToString()
    {
        return base.ToString() + $", Interest Rate={InterestRate:F2}%";
    }
}

// Main program
class InvestmentTracker
{
    static void Main()
    {
        // Display program header
        Console.WriteLine("************************************************************");
        Console.WriteLine("               INVESTMENT TRACKER VERSION 1.0");
        Console.WriteLine("************************************************************");
        Console.WriteLine("\nThis tool helps you manage your investments, both CDs and checking accounts.");
        Console.WriteLine("CDs accrue interest and checking accounts can have overdraft fees.\n");

        Console.Write("Enter your name: ");
        string name = Console.ReadLine();

        // Create accounts by collecting user input
        CheckingAccount checking = CreateCheckingAccount(name);
        CD cd = CreateCD(name);

        List<Investment> investments = new List<Investment> { checking, cd };

        // Main menu loop
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
                    if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount))
                    {
                        checking.Withdraw(withdrawAmount);
                    }
                    else
                    {
                        Console.WriteLine("Invalid amount. Please enter a valid number.");
                    }
                    break;
                case "2":
                    Console.Write("How much do you want to deposit? ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                    {
                        checking.Deposit(depositAmount);
                    }
                    else
                    {
                        Console.WriteLine("Invalid amount. Please enter a valid number.");
                    }
                    break;
                case "3":
                    // Apply automatic adjustments to all investments
                    foreach (var investment in investments)
                        investment.ApplyAdjustment();
                    Console.WriteLine("The CD accrued interest, and the checking account applied late fees if applicable.");
                    break;
                case "4":
                    // Display investment details
                    Console.WriteLine("Here are your investments at our bank:");
                    foreach (var investment in investments)
                        Console.WriteLine(investment);
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 5.");
                    break;
            }
        }
    }
}
