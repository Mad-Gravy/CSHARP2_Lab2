using System;
using System.Collections.Generic;
namespace BinaryNumbersQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            // Deliver instructions and store user's integer choice in a variable
            Console.WriteLine("Let's do some numeral-to-binary conversions! Sounds fun, right? RIIIIGHT?!");
            Console.WriteLine("Let's make this extra educational; you enter a positive whole number, and I'll show you all the binary numbers from 1 to the number you choose. CHOOSE WISELY!");
            Console.WriteLine("Enter a positive number for me:  ");
            int choice = int.Parse(Console.ReadLine());

            // Create a queue of numbers between 1 and user choice
            Queue<int> binaries = new Queue<int>();
            binaries.Enqueue(1); // Many thanks for putting this in the Review Form lol

            Console.WriteLine($"Alright, the binary numbers between 1 and {choice} would look like this :");

            for (int i = 0; i < choice; i++)
            {
                int number = binaries.Dequeue();

                // found this neat trick on stackOverflow @ https://stackoverflow.com/questions/2954962/convert-integer-to-binary-in-c-sharp?newreg=b2a9a835f5d54047a8739724868fb5cb
                Console.WriteLine(Convert.ToString(number, 2));

                binaries.Enqueue(number + 1);
            }
        }

    }
}