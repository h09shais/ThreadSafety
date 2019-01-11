using System;
using System.Collections.Generic;
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

            //for (var i = 0; i < tasks.Length; i++)
            //{
            //    tasks[i] = Task.Run(() => RandomlyUpdate(account));
            //}

            //Parallel.For(0, tasks.Length, i =>
            //{
            //    tasks[i] = Task.Run(() => RandomlyUpdate(account));
            //});

            //Task.WaitAll(tasks);

            Parallel.ForEach(
                tasks, 
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = 3
                }, 
                task =>
                {
                    var stopwatch = new CustomStopwatch();
                    stopwatch.Start();

                    RandomlyUpdate(account);

                    stopwatch.Stop();

                    if (stopwatch.StartAt.HasValue && stopwatch.EndAt.HasValue)
                    {
                        Console.WriteLine("Stopwatch elapsed: {0}, Start at: {1}, End at: {2}", stopwatch.ElapsedMilliseconds, stopwatch.StartAt.Value, stopwatch.EndAt.Value);
                    }
                    else
                    {   Console.WriteLine("Stopwatch elapsed: {0}", stopwatch.ElapsedMilliseconds);
                    }
                });

            Console.ReadKey();
        }

        public static async Task RunLimitedNumberAtATime<T>(int numberOfTasksConcurrent, IEnumerable<T> inputList, Func<T, Task> asyncFunc)
        {
            var inputQueue = new Queue<T>(inputList);
            var runningTasks = new List<Task>(numberOfTasksConcurrent);
            for (var i = 0; i < numberOfTasksConcurrent && inputQueue.Count > 0; i++)
            {
                runningTasks.Add(asyncFunc(inputQueue.Dequeue()));
            }

            while (inputQueue.Count > 0)
            {
                Task task = await Task.WhenAny(runningTasks);
                runningTasks.Remove(task);
                runningTasks.Add(asyncFunc(inputQueue.Dequeue()));
            }

            await Task.WhenAll(runningTasks);
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
