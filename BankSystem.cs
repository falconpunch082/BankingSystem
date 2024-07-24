using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace BankingSystem
{
    class BankSystem
    {
        // MenuOption enumeration
        public enum MenuOption
        {
            Withdraw,
            Deposit,
            Transfer,
            Print,
            AddAccount,
            PrintHistory,
            Quit
        }

        // Menu function
        public static MenuOption ReadUserOption()
        {
            // Declaring variables
            int input = 0;
            Boolean valid = false;

            // Do-while loop for input
            do
            {
                Console.Clear();

                // Print logo
                Console.WriteLine(" ooooo     ooo oooooooooooo oooooooooo.  ");
                Console.WriteLine("`888b.     `8' `888'     `8 `888'   `Y8b ");
                Console.WriteLine(" 8 `88b.    8   888          888     888 ");
                Console.WriteLine(" 8   `88b.  8   888oooo8     888oooo888' ");
                Console.WriteLine(" 8     `88b.8   888         888    `88b  ");
                Console.WriteLine(" 8       `888   888       o  888    .88P ");
                Console.WriteLine("o8o        `8  o888ooooood8 o888bood8P'  ");

                // Print options
                Console.WriteLine("\n--- New Eridu Bank System || Choose an option... ---");
                Console.WriteLine("1. Withdraw");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Transfer");
                Console.WriteLine("4. Print Account Details");
                Console.WriteLine("5. Add Account");
                Console.WriteLine("6. Display Transaction History");
                Console.WriteLine("7. Quit");
                Console.WriteLine("----------------------------------------------------");
                Console.Write("Enter option here: ");

                // Input + Validation - check that input is a number which is between 1 and 7 inclusive
                try
                {
                    input = Convert.ToInt32(Console.ReadLine());

                    if (input >= 1 & input <= 7)
                    {
                        // Do nothing and leave the loop
                        valid = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 7 inclusive ONLY.");
                        valid = false;
                        Console.WriteLine("\nEnter any key to acknowledge");
                        Console.ReadKey();
                        continue;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 7 inclusive ONLY.");
                    valid = false;
                    Console.WriteLine("\nEnter any key to acknowledge");
                    Console.ReadKey();
                    continue;
                }
            } while (!valid);

            // Returning enum based on option
            switch (input)
            {
                case 1:
                    Console.WriteLine("Withdraw option chosen...\n");
                    return MenuOption.Withdraw;
                case 2:
                    Console.WriteLine("Deposit option chosen...\n");
                    return MenuOption.Deposit;
                case 3:
                    Console.WriteLine("Transfer option chosen...\n");
                    return MenuOption.Transfer;
                case 4:
                    Console.WriteLine("Print option chosen...\n");
                    return MenuOption.Print;
                case 5:
                    Console.WriteLine("Adding new account...\n");
                    return MenuOption.AddAccount;
                case 6:
                    Console.WriteLine("Showing transaction history...\n");
                    return MenuOption.PrintHistory;
                case 7:
                    Console.WriteLine("Quitting application...\n");
                    return MenuOption.Quit;
                default: return MenuOption.Quit;
            }
        }

        // FindAccount function
        private static Account FindAccount(Bank b)
        {
            String input;
            bool valid = false;

            Console.WriteLine("Please select account to perform operation...");
            foreach (Account a in b.accounts)
            {
                Console.WriteLine("- " + a.Name + " (Balance : " + a.Balance.ToString("C") + ")");
            }

            Console.Write("Please enter full account name here: ");
            input = Console.ReadLine();
            foreach (Account a in b.accounts)
            {
                if (input == a.Name)
                {
                    valid = true;
                    return a;
                }
            }

            return null;
        }

        // Menu option functions
        static void DoDeposit(Bank b)
        {
            Account account = FindAccount(b);

            if (account == null)
            {
                Console.WriteLine("Account not found. Returning to menu.");
                return;
            }

            decimal amount;
            Console.Write("Please enter amount to deposit: ");
            amount = Convert.ToDecimal(Console.ReadLine());

            DepositTransaction d = new DepositTransaction(account, amount);
            d.Print();
            try
            {
                b.ExecuteTransaction(d);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("\nEnter any key to acknowledge");
            Console.ReadKey();
        }

        static void DoWithdraw(Bank b)
        {
            Account account = FindAccount(b);

            if (account == null)
            {
                Console.WriteLine("Account not found. Returning to menu.");
                return;
            }

            decimal amount;
            Console.Write("Please enter amount to withdraw: ");
            amount = Convert.ToDecimal(Console.ReadLine());

            WithdrawTransaction w = new WithdrawTransaction(account, amount);
            w.Print();
            try
            {
                b.ExecuteTransaction(w);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("\nEnter any key to acknowledge");
            Console.ReadKey();
        }

        static void DoTransfer(Bank b)
        {
            String input;
            bool validFrom = false;
            bool validTo = false;
            Account fromaccount = null;
            Account toaccount = null;

            // Setting fromaccount
            Console.WriteLine("-- Sender account selection ---");
            while (!validFrom)
            {
                fromaccount = FindAccount(b);

                if (fromaccount == null)
                {
                    Console.WriteLine("Invalid account. Please try again.");
                    validFrom = false;
                }
                else
                {
                    validFrom = true;
                }
            }

            // Setting fromaccount
            Console.WriteLine("\n-- Recipient account selection ---");
            while (!validTo)
            {
                toaccount = FindAccount(b);

                if (toaccount == null)
                {
                    Console.WriteLine("Invalid account. Please try again.");
                    validTo = false;
                }
                else
                {
                    validTo = true;
                }
            }

            decimal amount;
            Console.WriteLine($"\n{fromaccount.Name}'s current balance: {fromaccount.Balance.ToString("C")}");
            Console.Write("Please enter amount to transfer: ");
            amount = Convert.ToDecimal(Console.ReadLine());

            TransferTransaction t = new TransferTransaction(fromaccount, toaccount, amount);
            t.Print();
            try
            {
                b.ExecuteTransaction(t);
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("\nEnter any key to acknowledge");
            Console.ReadKey();
        }

        static void DoAddAccount(Bank bank)
        {
            String name = "";
            decimal amount = 0;
            Boolean nameValid = false;
            Boolean amountValid = false;

            while (!nameValid)
            {
                Console.Write("Please enter name under the new account (or type 'c' to cancel operation): ");
                name = Console.ReadLine();

                if (name == "C" | name == "c")
                {
                    Console.WriteLine("Cancelling operation");
                    Console.WriteLine("\nEnter any key to acknowledge");
                    Console.ReadKey();
                    return;
                }

                if (Regex.IsMatch(name, "^[A-Za-z ]+$")) // Checks if only letters are found
                {
                    nameValid = true;
                }
                else
                {
                    Console.WriteLine("Please enter only letters or spaces.");
                    nameValid = false;
                }
            }

            while (!amountValid)
            {
                Console.Write("Please enter balance of the new account: ");

                amount = Convert.ToDecimal(Console.ReadLine());
                if (amount >= 0)
                {
                    amountValid = true;
                }
                else
                {
                    Console.WriteLine("Please enter a value which is zero or higher.");
                    amountValid = false;
                }
            }

            Account n = new Account(amount, name);
            bank.AddAccount(n);
            Console.WriteLine($"Account under {name} with balance of {amount.ToString("C")} created.");

            Console.WriteLine("\nEnter any key to acknowledge");
            Console.ReadKey();
        }

        static void DoPrint(Bank b)
        {
            Account account = FindAccount(b);

            if (account == null)
            {
                Console.WriteLine("Account not found. Returning to menu.");
                return;
            }

            account.Print();

            Console.WriteLine("\nEnter any key to acknowledge");
            Console.ReadKey();
        }

        static void DoDisplay(Bank b)
        {
            int length = b.transactions.Count;

            b.PrintTransactionHistory();

            if (length != 0)
            {
                Console.WriteLine("\nWould you like to rollback a transaction? (type 'y' if yes, otherwise press enter to exit)");
                String input = Console.ReadLine();
                if (input == "Y" | input == "y")
                {
                    DoRollback(b);
                    Console.WriteLine("\nEnter any key to acknowledge");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Returning to menu.");
                    Console.WriteLine("\nEnter any key to acknowledge");
                    Console.ReadKey();
                    return;
                }
            } else 
            {
                Console.WriteLine("Returning to menu.");
                Console.WriteLine("\nEnter any key to acknowledge");
                Console.ReadKey();
                return;
            }
        }

        static void DoRollback(Bank b)
        {
            Console.Write("Please enter transaction number to rollback: ");

            int input;
            try
            {
                input = Convert.ToInt32(Console.ReadLine());
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Invalid input. Returning to menu.");
                return;
            }

            int length = b.transactions.Count;
            if (input > 0 & input <= length)
            {
                Transaction t = b.transactions[input - 1];
                Console.WriteLine("\nPerforming rollback.");
                try
                {
                    b.RollbackTransaction(t);
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Rollback unsuccessful.");
                }
            }
            else
            {
                Console.WriteLine("No such transaction. Returning to menu.");
                return;
            }
        }

        static void Main(string[] args)
        {
            // Create new Bank object to store accounts
            Bank bank = new Bank();

            // Create accounts
            Account tim = new Account(100, "Tim Stevens");
            Account rachel = new Account(250, "Rachel Monroe");
            Account joe = new Account(350, "Joe Doe");

            // Fill accounts List with the accounts
            bank.accounts.Add(tim);
            bank.accounts.Add(rachel);
            bank.accounts.Add(joe);

            // Assign current account as Tim's
            Account current = tim;

            // Start with menu 
            do
            {
                switch (ReadUserOption())
                {
                    case MenuOption.Withdraw:
                        DoWithdraw(bank);
                        break;
                    case MenuOption.Deposit:
                        DoDeposit(bank);
                        break;
                    case MenuOption.Transfer:
                        DoTransfer(bank);
                        break;
                    case MenuOption.Print:
                        DoPrint(bank);
                        break;
                    case MenuOption.AddAccount:
                        DoAddAccount(bank);
                        break;
                    case MenuOption.PrintHistory:
                        DoDisplay(bank);
                        break;
                    case MenuOption.Quit:
                        Environment.Exit(0);
                        break;
                }
            } while (true); // not an infinite loop as quit option will terminate console and therefore loop
        }
    }
 }