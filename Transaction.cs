using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem
{
    // Base transaction class
    abstract class Transaction
    {
        public Account _account;
        public decimal _amount = 0;
        protected bool _executed = false;
        public bool _success = false;
        protected bool _reversed = false;
        protected DateTime dateStamp;

        public bool Executed
        {
            get { return _executed; }
        }

        public virtual bool Success
        {
            get { return _success; }
        }

        public bool Reversed
        {
            get { return _reversed; } 
        }

        public DateTime DateStamp
        {
            get { return dateStamp; } 
        }

        public Transaction(decimal amount)
        {
            this._amount = amount;
        }

        public virtual void Print()
        {
            // Placeholder
        }

        public virtual void Execute()
        {
            // Placeholder
        }

        public virtual void Rollback()
        {
            // Placeholder
        }
    }

    // Withdrawal transaction class
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
            // First, changes _executed status to true
            _executed = true;

            // Debits associated account deducting the specified amount while checking if there is enough money
            if ((_account.Balance - _amount) >= 0) // check if after withdrawal balance is zero or more
            {
                _account.Balance -= _amount;
                dateStamp = DateTime.Now;
                _success = true;
                Console.WriteLine($"Withdrawal successful. The balance of the account under {_account.Name} is now {_account.Balance.ToString("C")}.");
            }
            else // case if with withdrawal balance is negative
            {
                throw new InvalidOperationException("Account does not have enough funds for withdrawal.");
            }
        }

        public override void Rollback()
        {
            if (_executed & !_reversed)
            {
                _account.Balance += _amount;
                dateStamp = DateTime.Now;
                _reversed = true;
                Console.WriteLine($"Withdrawal reversed. The balance of the account under {_account.Name} is now {_account.Balance.ToString("C")}.");
            } else if (!_executed)
            {
                throw new InvalidOperationException("Withdrawal has not been finalised.");
            }
            else if (_reversed)
            {
                throw new InvalidOperationException("Withdrawal has already been reversed.");
            }
        }
    }

    // Deposit transaction class
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
            // First, changes _executed status to true
            _executed = true;

            // Credits associated account adding the specified amount while checking if the amount deposited is more than zero
            if (_amount > 0) // check if after withdrawal balance is zero or more
            {
                _account.Balance += _amount;
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
            if (_executed & !_reversed)
            {
                _account.Balance -= _amount;
                dateStamp = DateTime.Now;
                _reversed = true;
                Console.WriteLine($"Deposit reversed. The balance of the account under {_account.Name} is now {_account.Balance.ToString("C")}.");
            }
            else if (!_executed)
            {
                throw new InvalidOperationException("Deposit has not been finalised.");
            }
            else if (_reversed)
            {
                throw new InvalidOperationException("Deposit has already been reversed.");
            }
        }
    }

    // Transfer transaction class
    class TransferTransaction : Transaction
    {
        // Declare more variables
        public Account _fromaccount = null;
        public Account _toaccount = null;
        private DepositTransaction _deposit;
        private WithdrawTransaction _withdraw;

        public TransferTransaction(Account fromaccount, Account toaccount, decimal amount) : base(amount)
        {
            _fromaccount = fromaccount;
            _toaccount = toaccount;
            _amount = amount;
            _deposit = new DepositTransaction(toaccount, amount);
            _withdraw = new WithdrawTransaction(fromaccount, amount);
        }

        public override bool Success
        {
            get
            {
                if (_deposit.Success & _withdraw.Success)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }

        public override void Print()
        {
            Console.WriteLine("\nCurrent operation: Transfer of " + _amount.ToString("C") + " from the account of " + _fromaccount.Name + " to the account of " + _toaccount.Name + ".");
        }

        public override void Execute()
        {
            // First, changes _executed status to true
            _executed = true;

            try
            {
                _withdraw.Execute();
                _deposit.Execute();
                _success = true;
                dateStamp = DateTime.Now;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Transfer failed to complete.");
            }
        }

        public override void Rollback()
        {
            if (_executed & !_reversed)
            {
                try
                {
                    _deposit.Rollback();
                    _withdraw.Rollback();
                    dateStamp = DateTime.Now;
                    _reversed = true;
                    Console.WriteLine($"Transfer reversed.");
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Transfer rollback failed to complete.");
                }
            }
            else if (!_executed)
            {
                throw new InvalidOperationException("Transfer has not been finalised.");
            }
            else if (_reversed)
            {
                throw new InvalidOperationException("Transfer has already been reversed.");
            }
        }
    }
}
