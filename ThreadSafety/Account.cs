using System;

namespace ThreadSafety
{
    public class Account
    {
        private readonly object _balanceLock = new object();
        private decimal _balance;

        public Account(decimal initialBalance)
        {
            _balance = initialBalance;
        }

        public decimal Debit(decimal amount)
        {
            lock (_balanceLock)
            {
                if (_balance < amount) return 0;

                Console.WriteLine($"Balance before debit :{_balance,5}");
                Console.WriteLine($"Amount to remove     :{amount,5}");
                _balance = _balance - amount;
                Console.WriteLine($"Balance after debit  :{_balance,5}");
                Console.WriteLine("____________________________________");

                return amount;
            }
        }

        public void Credit(decimal amount)
        {
            lock (_balanceLock)
            {
                Console.WriteLine($"Balance before credit:{_balance,5}");
                Console.WriteLine($"Amount to add        :{amount,5}");
                _balance = _balance + amount;
                Console.WriteLine($"Balance after credit :{_balance,5}");
                Console.WriteLine("___________________________________");
            }
        }
    }
}
