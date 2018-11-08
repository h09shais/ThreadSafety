using System;
using System.Threading.Tasks;

namespace ThreadSafety
{
    class Program
    {
        static void Main(string[] args)
        {
            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/lock-statement

            var account = new Account(1000);
            var tasks = new Task[10];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() => RandomlyUpdate(account));
            }
            Task.WaitAll(tasks);

            Console.ReadKey();
        }

        static void RandomlyUpdate(Account account)
        {
            var rnd = new Random();
            for (var i = 0; i < 10; i++)
            {
                var amount = rnd.Next(1, 100);
                var doCredit = rnd.NextDouble() < 0.5;
                if (doCredit)
                {
                    account.Credit(amount);
                }
                else
                {
                    account.Debit(amount);
                }
            }
        }
    }
}
