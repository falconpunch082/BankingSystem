using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem
{
    class WithdrawTransaction : Transaction
    {
        public WithdrawTransaction(Account account, decimal amount) : base(amount)
        {
            _amount = amount;
            _account = account;
        }

        public override void Print()
        {
            Console.WriteLine("\nCurrent operation: Withdrawal of " + _amount.ToString("C") + " from the account of " + _account.Name + ".");
        }

        public override void Execute()
        {
            base.Execute();

            // Debits associated account deducting the specified amount while checking if there is enough money
            if (_account.Withdraw(_amount)) // check if after withdrawal balance is zero or more
            {
                dateStamp = DateTime.Now;
                _success = true;
                Console.WriteLine($"Withdrawal successful. The balance of the account under {_account.Name} is now {_account.Balance.ToString("C")}.");
            }
            else // case if with withdrawal balance is negative
            {
                throw new InvalidOperationException("Account does not have enough funds for withdrawal OR withdrawal amount invalid.");
            }
        }

        public override void Rollback()
        {
            base.Rollback();

            _account.Deposit(_amount);
            dateStamp = DateTime.Now;
            _reversed = true;
            Console.WriteLine($"Withdrawal reversed. The balance of the account under {_account.Name} is now {_account.Balance.ToString("C")}.");
        }
    }
}
