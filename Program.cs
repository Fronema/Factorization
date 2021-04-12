using System;


namespace Factorization
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Give me Product of two primes:");
            string input = Console.ReadLine();
            int product = int.Parse(input);

            var solver = new ShorsSolver(product);
            solver.Solve();
        }

    }
}
