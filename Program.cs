using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        long limit = 2;
        var stopwatch = new Stopwatch();

        Console.WriteLine("Press 'C' key to exit the loop...");

        while (true)
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.C)
            {
                break;
            }

            limit++;
            stopwatch.Start();

            // Call the asynchronous method  2_147_483_600
            var maxPrime = await MaxPrimeLessThanAsync(limit);
            
            stopwatch.Stop();

            // Print the result
            Console.Write($"Max prime less than {limit,10} is {maxPrime,10} ");

            Console.Write($"task took {stopwatch.ElapsedMilliseconds,5} ms.\r");
        }
    }

    public static  ValueTask<long> MaxPrimeLessThanAsync(long number)
    {
        if (number < 2_000_000_000)
        {
            return new ValueTask<long>(MaxPrimeLessThan(number));
        }
        else
        {
            return new ValueTask<long>(Task.Run(() => MaxPrimeLessThan(number)));
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