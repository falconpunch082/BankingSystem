using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem
{
    class Bank
    {
        // Creating a List of Accounts
        public List<Account> accounts = new List<Account>();

        // Creating a List of Transactions done
        public List<Transaction> transactions = new List<Transaction>();

        // Empty constructor
        public Bank()
        {

        }
        
        // Method to add an account into the accounts List
        public void AddAccount(Account account)
        {
            accounts.Add(account);
        }

        // Method to retrieve an account from the accounts List based on the name provided to the account
        public Account GetAccount(String name)
        {
            foreach (Account a in accounts)
            {
                if (name == a.Name)
                {
                    return a;
                }
            }

            return null;
        }

        // Method to execute the provided transaction
        public void ExecuteTransaction(Transaction t)
        {
            t.Execute();
            transactions.Add(t);
        }

        // Method to execute the provided transaction
        public void RollbackTransaction(Transaction t)
        {
            t.Rollback();
        }

        public void PrintTransactionHistory()
        {
            int length = transactions.Count;

            Console.WriteLine("--- Transaction History ---");

            if (length != 0 )
            {
                Console.WriteLine("(Start of history)");

                foreach (Transaction t in transactions)
                {
                    String status;
                    if (t.Executed == true)
                    {
                        if (t.Success == true)
                        {
                            if (t.Reversed == true)
                            {
                                status = "Reversed";
                            }
                            else
                            {
                                status = "Success";
                            }
                        }
                        else
                        {
                            status = "Failure";
                        }
                    }
                    else
                    {
                        status = "Not executed";
                    }

                    if (t.GetType().Equals(typeof(DepositTransaction)))
                    {
                        Console.WriteLine($"{transactions.IndexOf(t) + 1} | Deposit of {t._amount.ToString("C")} to the account under {t._account.Name} - [STATUS: {status} (as of {t.DateStamp})]");
                    }
                    else if (t.GetType().Equals(typeof(WithdrawTransaction)))
                    {
                        Console.WriteLine($"{transactions.IndexOf(t) + 1} | Withdrawal of {t._amount.ToString("C")} from the account under {t._account.Name} - [STATUS: {status} (as of {t.DateStamp})]");
                    }
                    else if (t.GetType().Equals(typeof(TransferTransaction)))
                    {
                        TransferTransaction transfer = (TransferTransaction)t;
                        Console.WriteLine($"{transactions.IndexOf(t) + 1} | Transfer of {t._amount.ToString("C")} from {transfer._fromaccount.Name} to {transfer._toaccount.Name} - [STATUS: {status} (as of {t.DateStamp})]");
                    }

                    Console.WriteLine("(End of history)");
                }
            } else
            {
                Console.WriteLine("No recorded transaction history.");
            }
        }
    }
}
