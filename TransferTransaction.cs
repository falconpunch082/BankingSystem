using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem
{
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
                }
                else
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
            base.Execute();

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
            base.Rollback();

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
    }
}
