namespace ValueTaskExample
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main()
        {
            long limit = 2;
            bool enteredNoGCRegion = false;
            
            enteredNoGCRegion = GC.TryStartNoGCRegion((int.MaxValue)/10);
            var stopwatch = new Stopwatch();
            while (true)
            {                
                limit++;                
                stopwatch.Start();

                // Call the asynchronous method  2_147_483_600
                Task<long> maxPrimeTask = MaxPrimeLessThanAsync(limit);                

                // Wait for the task to complete to prevent the program from exiting prematurely
                maxPrimeTask.Wait();                
                stopwatch.Stop();
                
                // Print the result
                Console.Write($"Max prime less than {limit,10} is {maxPrimeTask.Result,10} ");

                Console.Write($"task {stopwatch.ElapsedMilliseconds, 5} ms.");
                // Print the current memory usage
                long memoryUsed = Process.GetCurrentProcess().WorkingSet64;
                Console.Write($" memory usage: {memoryUsed: 10}\r");
            }
            if (enteredNoGCRegion)
            {
                GC.EndNoGCRegion();
            }
        }

        public static Task<long> MaxPrimeLessThanAsync(long number)
        {
            if (number < 2_000_000_000)
            {
                return Task.FromResult(MaxPrimeLessThan(number));
            }
            else
            {
                return Task.Run(() => MaxPrimeLessThan(number));
            }
        }

        private static long MaxPrimeLessThan(long n)
        {
            for (long i = n - 1; i >= 2; i--)
            {
                if (IsPrime(i))
                {
                    return i;
                }
            }

            return -1; // Return -1 if no prime found (i.e., when n is less than 2)
        }

        private static bool IsPrime(long n)
        {
            if (n < 2) return false;

            for (int i = 2; i * i <= n; i++)
            {
                if (n % i == 0) return false;
            }

            return true;
        }
    }
}
