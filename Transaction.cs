using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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

        abstract public void Print(); // Removes need for placeholder

        public virtual void Execute()
        {
            // Changes _executed status to true
            _executed = true;
        }

        public virtual void Rollback()
        {
            // Checks if transaction has been reversed or not executed
            if (!_executed)
            {
                throw new InvalidOperationException("Transaction has not been finalised.");
            }
            else if (_reversed)
            {
                throw new InvalidOperationException("Tranasction has already been reversed.");
            }
            else if (_executed & !_reversed)
            {
                
            }
        }
    }
}
