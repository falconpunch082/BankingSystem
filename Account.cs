using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem
{
    class Account
    {
        // Declaring variables
        private decimal _balance;
        private String _name;

        // Property
        public String Name
        {
            get { return _name; }
        }

        public decimal Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }

        // Constructor
        public Account(decimal balance, string name)
        {
            this._balance = balance;
            this._name = name;
        }

        // Methods
        public void Print()
        {
            Console.WriteLine("\n--- Account Details ---");
            Console.WriteLine("Name Under: " + this._name);
            Console.WriteLine("Balance: " + this._balance.ToString("C"));
            Console.WriteLine("-----------------------\n");
        }
    }
}
