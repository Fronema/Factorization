using System;
using System.Collections.Generic;
using System.Numerics;

namespace Factorization
{
    public class ShorsSolver
    {
        BigInteger Product { get; set; }
        int LastRandomGuess { get; set; }

        public ShorsSolver(int product)
        { 
            Product = product;
        }


        public void Solve()
        {
            int guess = 6;//(int)Math.Floor(Math.Sqrt(Product));
            LastRandomGuess = guess;
            WriteOut($"My first guess if {guess}");//, because sqrt is {Math.Sqrt(Product)}");

            FindFactors(guess);
        }
       
        private void FindFactors(BigInteger guess, bool continuing = false)
        {
            WriteOut($"Find factors: Starting.");  //  Product is {Product} and guess is {guess}.
            WriteOut($"GetBiggestCommonDenominator: Starting.");
            BigInteger denominator = GetBiggestCommonDenominator(Product, guess);
            if (denominator != 0)
            {
                //Win
                WriteOut($"Find factors: Factors of Product are: {denominator} and {Product / denominator}. For check {denominator * Product / denominator}. ");
                Console.ReadLine();
                return;
            }

            BigInteger P = FindP(guess);
            if (P > 0)  // It found something sensible
            {
                var halfP = (int)(P / 2);
                var newGuess = BigInteger.Pow(guess, halfP) + 1;
                WriteOut($"Find factors: Trying with another guess which is {guess}^{P}/2 + 1 = {newGuess}.");
                FindFactors(newGuess, true);

                newGuess = BigInteger.Pow(guess, halfP) - 1;
                WriteOut($"Find factors: That didnt work Trying with another guess which is {guess}^{P}/2 - 1 = {newGuess}.");          
                FindFactors(newGuess);
            }
            else if (continuing == false)
            {  //starting again with random guess
                LastRandomGuess++;
                var newGuess = LastRandomGuess;
                WriteOut($"Find factors: I'll try again with new random guess {newGuess} ----------------------------------------------");

                FindFactors(newGuess);
            }
        }


        //this is what will take most of the time
        private BigInteger FindP(BigInteger guess)
        {
            WriteOut($"FindP: Starting");// with Product {Product} and guess {guess}");
            var listOfModulos = new List<int>();

            for (int i = 3; i < int.MaxValue; i++)
            {
                var poweredGuess = BigInteger.Pow(guess, i);
                if (poweredGuess < Product)
                {
                  //  WriteOut($"FindP: Skipping {i} because {poweredGuess} ({guess}^{i}) is less then Product {Product}");
                    continue;
                }
                
                int currentModulo = (int)(poweredGuess % Product);

                WriteProgress($"FindP: I tried {listOfModulos.Count} different modulos");    
                //       WriteOut($"FindP: We tried {guess} to power {i} and it is {poweredGuess} and modulo is {currentModulo}.");
                     
                if (listOfModulos.Contains(currentModulo))
                {
                    int indexOfFirstOne = listOfModulos.IndexOf(currentModulo);
                    int P = i - indexOfFirstOne;
                    WriteOut($"FindP: We found a repeated modulo {currentModulo} first index was {indexOfFirstOne} and current is {i} so P = {P}");
                    if (P % 2 == 0)
                    {
                        return P;
                    }
                    else
                    {
                        WriteOut($"FindP: This guess resulted in odd P {P}, try again with different guess.");
                        return -1;
                    }
                }
                listOfModulos.Add(currentModulo);
            }
            WriteOut($"FindP: Too much man, too much");
            return 0;
        }

        // euclids algorithm
        static BigInteger GetBiggestCommonDenominator(BigInteger bigger, BigInteger smaller)
        {           
            if (bigger < smaller)
            {
                BigInteger a = smaller;
                smaller = bigger;
                bigger = a;
            }

            BigInteger modulo = bigger % smaller;
      //      WriteOut($"GetBiggestCommonDenominator: {bigger} and {smaller}. Modulo is {modulo}");

            if (modulo == 1)
            {
                WriteOut($"GetBiggestCommonDenominator: There is no common denominator");
                return 0;
            }
            else if (modulo == 0)
            {
                WriteOut($"GetBiggestCommonDenominator: I found biggest common denominator and it is {smaller}");
                return smaller;
            }
            else
            {
                return GetBiggestCommonDenominator(smaller, modulo);
            }
        }

        static void WriteOut(string text)
        {
            Console.WriteLine($"{DateTime.Now.TimeOfDay}: {text}");

            //Console.ReadLine();
        }

        private void WriteProgress(string s)
        {
            int origRow = Console.CursorTop;
            int origCol = Console.CursorLeft;
            Console.Write(s);
            Console.SetCursorPosition(origCol, origRow);
        }
    }
}
