using System;
using System.Collections.Generic;

class Account
{
    public string Name { get; set; }
    public double Balance { get; set; }

    public Account(string name, double balance)
    {
        Name = name;
        Balance = balance;
    }

    public virtual bool Deposit(double amount)
    {
        if (amount <= 0) return false;
        Balance += amount;
        return true;
    }

    public virtual bool Withdraw(double amount)
    {
        if (amount <= 0 || amount > Balance) return false;
        Balance -= amount;
        return true;
    }

    public override string ToString() => $"{Name} - Balance: {Balance}";
}

class SavingsAccount : Account
{
    public double InterestRate { get; set; }

    public SavingsAccount(string name, double balance, double interestRate = 5.0)
        : base(name, balance)
    {
        InterestRate = interestRate;
    }

    public override bool Deposit(double amount)
    {
        double interest = amount * InterestRate / 100;
        return base.Deposit(amount + interest);
    }

    public override string ToString() =>
        $"{Name} [Savings] - Balance: {Balance}, Interest: {InterestRate}%";
}

class CheckingAccount : Account
{
    private const double Fee = 1.5;

    public CheckingAccount(string name, double balance)
        : base(name, balance)
    {
    }

    public override bool Withdraw(double amount)
    {
        return base.Withdraw(amount + Fee);
    }

    public override string ToString() =>
        $"{Name} [Checking] - Balance: {Balance}";
}

class TrustAccount : SavingsAccount
{
    private int withdrawals = 0;
    private const int MaxWithdrawals = 3;

    public TrustAccount(string name, double balance, double interestRate = 5.0)
        : base(name, balance, interestRate)
    {
    }

    public override bool Deposit(double amount)
    {
        if (amount >= 5000)
            amount += 50;
        return base.Deposit(amount);
    }

    public override bool Withdraw(double amount)
    {
        if (withdrawals >= MaxWithdrawals || amount > Balance * 0.2)
            return false;

        withdrawals++;
        return base.Withdraw(amount);
    }

    public override string ToString() =>
        $"{Name} [Trust] - Balance: {Balance}, Withdrawals: {withdrawals}/3";
}

class AccountUtil
{
   
    public static void Deposit<T>(List<T> accounts, double amount) where T : Account
    {
        Console.WriteLine($"\nDepositing {amount} to {typeof(T).Name}s:");
        foreach (var acc in accounts)
        {
            bool result = acc.Deposit(amount);
            Console.WriteLine(result ? $"Deposited to {acc}" : $"Failed to deposit to {acc.Name}");
        }
    }

    
    public static void Withdraw<T>(List<T> accounts, double amount) where T : Account
    {
        Console.WriteLine($"\nWithdrawing {amount} from {typeof(T).Name}s:");
        foreach (var acc in accounts)
        {
            bool result = acc.Withdraw(amount);
            Console.WriteLine(result ? $"Withdrew from {acc}" : $"Failed to withdraw from {acc.Name}");
        }
    }
}

class Program
{
    static void Main()
    {
        var savings = new List<SavingsAccount>
        {
            new SavingsAccount("Ali", 1000),
            new SavingsAccount("Laila", 1500)
        };

        var checking = new List<CheckingAccount>
        {
            new CheckingAccount("Yousef", 2000),
            new CheckingAccount("Mira", 3000)
        };

        var trust = new List<TrustAccount>
        {
            new TrustAccount("Noor", 10000),
            new TrustAccount("Amr", 8000)
        };

        AccountUtil.Deposit(savings, 1000);
        AccountUtil.Withdraw(savings, 500);

        AccountUtil.Deposit(checking, 500);
        AccountUtil.Withdraw(checking, 600);

        AccountUtil.Deposit(trust, 6000);
        AccountUtil.Withdraw(trust, 1500);
        AccountUtil.Withdraw(trust, 1500);
        AccountUtil.Withdraw(trust, 1500);
        AccountUtil.Withdraw(trust, 1500); 

        Console.WriteLine("\nFinal States:");
        foreach (var acc in savings) Console.WriteLine(acc);
        foreach (var acc in checking) Console.WriteLine(acc);
        foreach (var acc in trust) Console.WriteLine(acc);
    }
}
