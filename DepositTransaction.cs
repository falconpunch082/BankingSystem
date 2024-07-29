using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem
{
    class DepositTransaction : Transaction
    {
        public DepositTransaction(Account account, decimal amount) : base(amount)
        {
            _amount = amount;
            _account = account;
        }

        public override void Print()
        {
            Console.WriteLine("\nCurrent operation: Deposit of " + _amount.ToString("C") + " from the account of " + _account.Name + ".");
        }

        public override void Execute()
        {
            base.Execute();

            // Credits associated account adding the specified amount while checking if the amount deposited is more than zero
            if (_account.Deposit(_amount)) // check if after withdrawal balance is zero or more
            {
                dateStamp = DateTime.Now;
                _success = true;
                Console.WriteLine($"Deposit successful. The balance of the account under {_account.Name} is now {_account.Balance.ToString("C")}.");
            }
            else // case if amount is less or equal to zero
            {
                throw new InvalidOperationException("Invalid amount to deposit. Deposit amount should be more than zero.");
            }
        }

        public override void Rollback()
        {
            base.Rollback();

            _account.Withdraw(_amount);
            dateStamp = DateTime.Now;
            _reversed = true;
            Console.WriteLine($"Deposit reversed. The balance of the account under {_account.Name} is now {_account.Balance.ToString("C")}.");
        }
    }
}
